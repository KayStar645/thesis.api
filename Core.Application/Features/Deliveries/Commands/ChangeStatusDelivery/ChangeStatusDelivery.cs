using Core.Application.Common.Interfaces;
using Core.Application.Features.Deliveries.Events;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.Delivery;

namespace Core.Application.Features.Deliveries.Commands.ChangeStatusDelivery
{
    public record ChangeStatusDeliveryCommand : IRequest<Result<bool>>
    {
        public int? DeliveryId { get; set; }

        public DeliveryStatus? Status { get; set; }
    }

    public class ChangeStatusDeliveryCommandHandler : IRequestHandler<ChangeStatusDeliveryCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public ChangeStatusDeliveryCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(ChangeStatusDeliveryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ChangeStatusDeliveryValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                var delivery = await _context.Deliveries.FindAsync(request.DeliveryId);

                DeliveryStatus? oldStatus = delivery.Status;

                delivery.Status = request.Status;
                delivery.PackingStaffId = _currentUserService.StaffId;

                _context.Deliveries.Update(delivery);
                await _context.SaveChangesAsync(cancellationToken);

                // Sự kiện sau khi xác nhận đơn hàng
                await _mediator.Publish(new AfterChangeStatusDeliveryEvent(request, _currentUserService.StaffId));

                return Result<bool>.Success(true, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
