using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Orders.Commands.AddProductToCart;
using Core.Application.Features.Orders.Commands.BaseOrders;
using Core.Application.Features.Orders.Events;
using Core.Application.Responses;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Orders.Commands.RemoveProductInCart
{
    public record RemoveProductInCartCommand : IRequest<Result<bool>>
    {
        public int? ProductId { get; set; }
    }

    public class RemoveProductInCartCommandHandler : IRequestHandler<RemoveProductInCartCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public RemoveProductInCartCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(RemoveProductInCartCommand request, CancellationToken cancellationToken)
        {
            // Kiểm tra nếu không phải người dùng thì không có quyền
            if (_currentUserService.Type != CLAIMS_VALUES.TYPE_USER)
            {
                return Result<bool>.Failure("Vui lòng đăng ký tài khoản người dùng để đặt hàng!", StatusCodes.Status403Forbidden);
            }

            try
            {
                // Lấy giỏ hàng của người dùng
                var cart = await _context.Orders
                    .Include(x => x.Customer).ThenInclude(x => x.User)
                    .Where(x => x.Customer.User.Id == _currentUserService.UserId &&
                                x.Status == Order.OrderStatus.Cart)
                    .FirstOrDefaultAsync();

                var validator = new RemoveProductInCartCommandValidator(_context, cart.Id);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                var detailOrder = await _context.DetailOrders
                            .Where(x => x.ProductId == request.ProductId &&
                                        x.OrderId == cart.Id)
                            .FirstOrDefaultAsync();
                _context.DetailOrders.Remove(detailOrder);
                await _context.SaveChangesAsync(cancellationToken);

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
