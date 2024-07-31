using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Orders.Events;
using Core.Application.Responses;
using Core.Application.Services;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.Order;

namespace Core.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand : IRequest<Result<bool>>
    {
        public string? Message { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public CreateOrderCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CreateOrderValidator(_context, _currentUserService.CustomerId);
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

                // Lấy giỏ hàng của người dùng này
                var cart = await _context.Orders
                    .Where(x => x.CustomerId == _currentUserService.CustomerId &&
                                x.Status == OrderStatus.Cart)
                    .FirstOrDefaultAsync();

                if(cart == null)
                {
                    return Result<bool>.Failure("Vui lòng thêm sản phẩm vào giỏ hàng trước khi đặt hàng!", StatusCodes.Status400BadRequest);
                }

                // Tạo đơn hàng
                DateTime create = DateTime.Now;
                var order = new Order
                {
                    InternalCode = CommonService.InternalCodeGeneration("ORDER", create),
                    Date = create,
                    TotalAmount = cart.TotalAmount,
                    TotalDecrease = cart.TotalDecrease,
                    Total = cart.Total,
                    Message = request.Message,
                    Status = Order.OrderStatus.Order,
                    Type = Order.OrderType.Online,
                    IsPay = false,
                    CustomerId = _currentUserService.CustomerId,
                    CouponId = cart.CouponId,
                };
                var orderEntity = await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync(cancellationToken);

                // Tạo chi tiết đơn hàng
                var details = await _context.DetailOrders
                        .Where(x => x.OrderId == cart.Id &&
                                    x.IsSelected == true)
                        .Select(x => new DetailOrder
                        {
                            Id = x.Id,
                            Cost = x.Cost,
                            ReducedPrice = x.ReducedPrice,
                            Price = x.Price,
                            Quantity = x.Quantity,
                            ProductId = x.ProductId,
                            OrderId = orderEntity.Entity.Id,
                            GroupPromotion = x.GroupPromotion,
                        }).ToListAsync();

                if (details == null)
                {
                    return Result<bool>.Failure("Vui lòng thêm sản phẩm vào giỏ hàng trước khi đặt hàng!", StatusCodes.Status400BadRequest);
                }

                _context.DetailOrders.UpdateRange(details);
                await _context.SaveChangesAsync(cancellationToken);
                
                // Cập nhật lại thành tiền cho đơn hàng
                //order.TotalAmount = details.Sum(x => x.Cost * x.Quantity ?? 0);
                //order.TotalDecrease = details.Sum(x => x.ReducedPrice * x.Quantity ?? 0);
                //order.Total = details.Sum(x => x.Price * x.Quantity ?? 0);
                //_context.Orders.Update(order);
                //await _context.SaveChangesAsync(cancellationToken);

                await _mediator.Publish(new AfterCreateOrderEvent(cart.Id, details.Select(x => x.ProductId).ToList()));

                return Result<bool>.Success(true, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
