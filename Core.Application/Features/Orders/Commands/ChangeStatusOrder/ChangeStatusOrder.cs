using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Orders.Events;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.Order;

namespace Core.Application.Features.Orders.Commands.ChangeStatusOrder
{
    public record ChangeStatusOrderCommand : IRequest<Result<bool>>
    {
        public int? OrderId { get; set; }

        public OrderStatus? Status { get; set; }
    }

    public class ChangeStatusOrderCommandHandler : IRequestHandler<ChangeStatusOrderCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public ChangeStatusOrderCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(ChangeStatusOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ChangeStatusOrderValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                var order = await _context.Orders.FindAsync(request.OrderId);

                OrderStatus? oldStatus = order.Status;

                order.Status = request.Status;
                if(_currentUserService.Type == CONSTANT_CLAIM_TYPES.Staff)
                {
                    order.StaffApprovedId = _currentUserService.StaffId;
                }

                _context.Orders.Update(order);
                await _context.SaveChangesAsync(cancellationToken);

                // Sự kiện sau khi xác nhận đơn hàng
                await _mediator.Publish(new AfterChangeStatusOrderEvent(request, oldStatus));

                return Result<bool>.Success(true, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
