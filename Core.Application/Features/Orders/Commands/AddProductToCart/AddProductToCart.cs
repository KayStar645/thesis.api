using AutoMapper;
using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Orders.Commands.BaseOrders;
using Core.Application.Features.Orders.Events;
using Core.Application.Responses;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Orders.Commands.AddProductToCart
{
    public record AddProductToCartCommand : IRequest<Result<bool>>
    {
        public int? ProductId { get; set; }

        public int? Quantity { get; set; }
    }

    public class AddProductToCartCommandHandler : IRequestHandler<AddProductToCartCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public AddProductToCartCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new AddProductToCartValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                // Kiểm tra nếu không phải người dùng thì không có quyền
                if(_currentUserService.Type != CLAIMS_VALUES.TYPE_USER)
                {
                    return Result<bool>.Failure("Vui lòng đăng ký tài khoản người dùng để đặt hàng!", StatusCodes.Status403Forbidden);
                }

                await _mediator.Publish(new BeforeAddProductToCartEvent
                    (_currentUserService.UserId));

                // Lấy giỏ hàng của người dùng
                var cart = await _context.Orders
                    .Include(x => x.Customer).ThenInclude(x => x.User)
                    .Where(x => x.Customer.User.Id == _currentUserService.UserId &&
                                x.Status == Order.OrderStatus.Cart)
                    .FirstOrDefaultAsync();

                var detail = await _context.DetailOrders
                            .Where(x => x.ProductId == request.ProductId &&
                                        x.OrderId == cart.Id)
                            .FirstOrDefaultAsync();

                var product = await _context.Products.FindAsync(request.ProductId);

                // Nếu sp đã có cộng dồn số lượng
                if (detail != null)
                {
                    var quantity = detail.Quantity + request.Quantity;

                    if (quantity > product.Quantity)
                    {
                        return Result<bool>.Failure("Số lượng sản phẩm tồn kho không đủ!", StatusCodes.Status400BadRequest);
                    }

                    detail.Quantity = detail.Quantity + request.Quantity;
                    detail.Profit = null;
                    _context.DetailOrders.Update(detail);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    if(request.Quantity > product.Quantity)
                    {
                        return Result<bool>.Failure("Số lượng sản phẩm tồn kho không đủ!", StatusCodes.Status400BadRequest);
                    }

                    int? quantity = request.Quantity;

                    // Áp dụng chương trình khuyến mãi cho sản phẩm đơn
                    (Promotion promo, decimal? priceDiscoutMax, int? group) = 
                        await BaseOrderApplyPromotion.
                            ApplyPromotionForSingleProduct(_context, product);

                    var newDetail = new DetailOrder
                    {
                        OrderId = cart.Id,
                        ProductId = request.ProductId,
                        Quantity = quantity,
                        Cost = product.Price,
                        ReducedPrice = priceDiscoutMax,
                        Price = product.Price - priceDiscoutMax,
                        Profit = null,
                        IsSelected = true,
                        GroupPromotion = group,
                    };
                    await _context.DetailOrders.AddAsync(newDetail);
                    await _context.SaveChangesAsync(cancellationToken);

                    await _mediator.Publish(new AfterAddProductToCartEvent(_currentUserService.CustomerId));
                }

                await _mediator.Publish(new AfterUpdateProductInCartEvent(cart.Id));

                return Result<bool>.Success(true, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
