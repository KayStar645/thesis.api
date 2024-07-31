using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Orders.Commands.CancelOrder
{
    public record CancelOrderCommand : IRequest<Result<bool>>
    {
        public int? OrderId { get; set; }
    }

    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public CancelOrderCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new CancelOrderValidator(_context, _currentUserService.CustomerId);
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

                var order = await _context.Orders.FindAsync(request.OrderId);

                order.Status = Order.OrderStatus.Cancel;

                _context.Orders.Update(order);
                await _context.SaveChangesAsync(cancellationToken);                

                return Result<bool>.Success(true, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
