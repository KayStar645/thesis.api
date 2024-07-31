using Core.Application.Common.Interfaces;
using static Core.Domain.Entities.Delivery;

namespace Core.Application.Features.Deliveries.Commands.ChangeStatusDelivery
{
    public class ChangeStatusDeliveryValidator : AbstractValidator<ChangeStatusDeliveryCommand>
    {
        public ChangeStatusDeliveryValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.DeliveryId)
                .MustAsync(async (DeliveryId, token) =>
                {
                    return await pContext.Deliveries
                            .AnyAsync(x => x.Id == DeliveryId);
                }).WithMessage("Id đơn hàng không hợp lệ!");

            RuleFor(x => x.Status)
                .MustAsync(async (request, status, token) =>
                {
                    var prepare = await pContext.Deliveries.FindAsync(request.DeliveryId);

                    if (prepare == null)
                    {
                        return false;
                    }

                    if (!((prepare.Status == DeliveryStatus.Prepare && 
                           status == DeliveryStatus.Transport) ||

                          (prepare.Status == DeliveryStatus.Transport &&
                          (status == DeliveryStatus.Delivered) ||

                          (prepare.Status == DeliveryStatus.Delivered &&
                          (status == DeliveryStatus.Received ||
                           status == DeliveryStatus.Cancel)))))
                    {
                        return false;
                    }

                    return true;
                }).WithMessage("Trạng thái thay đổi đơn hàng không hợp lệ!");
        }
    }
}
