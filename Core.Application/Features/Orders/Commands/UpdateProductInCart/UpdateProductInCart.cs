using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Orders.Events;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.Order;

namespace Core.Application.Features.Orders.Commands.UpdateProductInCart
{
    public record UpdateProductInCartCommand : IRequest<Result<bool>>
    {
        public int? ProductId { get; set; }

        public int? Quantity { get; set; }

        public bool? IsSelected { get; set; }
    }

    public class UpdateProductInCartCommandHandler : IRequestHandler<UpdateProductInCartCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMapper _mapper;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public UpdateProductInCartCommandHandler(
            ISupermarketDbContext pContext,
            IMapper pMapper,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(UpdateProductInCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new UpdateProductInCartValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                // Kiểm tra nếu không phải người dùng thì không có quyền
                if (_currentUserService.Type != CLAIMS_VALUES.TYPE_USER)
                {
                    return Result<bool>.Failure("Vui lòng đăng ký tài khoản người dùng để đặt hàng!", StatusCodes.Status403Forbidden);
                }

                // Lấy giỏ hàng của người dùng
                var cart = await _context.Orders
                    .Include(x => x.Customer).ThenInclude(x => x.User)
                    .Where(x => x.Customer.User.Id == _currentUserService.UserId &&
                                x.Status == OrderStatus.Cart)
                    .FirstOrDefaultAsync();

                var detail = await _context.DetailOrders
                            .Where(x => x.ProductId == request.ProductId &&
                                        x.OrderId == cart.Id)
                            .FirstOrDefaultAsync();

                // Cập nhật lại số lượng sản phẩm và lợi nhuận
                if (detail != null)
                {
                    var product = await _context.Products.FindAsync(request.ProductId);
                    int? quantity = request.Quantity;

                    detail.Quantity = request.Quantity;
                    detail.IsSelected = request.IsSelected == true ? true : false;
                    _context.DetailOrders.Update(detail);
                    await _context.SaveChangesAsync(cancellationToken);
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
