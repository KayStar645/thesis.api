using Core.Application.Common.Interfaces;
using Core.Application.Features.Deliveries.Commands.ChangeStatusDelivery;
using Microsoft.Extensions.Logging;
using static Core.Domain.Entities.Delivery;

namespace Core.Application.Features.Deliveries.Events
{
    public class AfterChangeStatusDeliveryEvent : INotification
    {
        public ChangeStatusDeliveryCommand? Request { get; set; }

        public int? StaffId { get; set; }

        public AfterChangeStatusDeliveryEvent(ChangeStatusDeliveryCommand pRquest,
            int? pStaffId)
        {
            Request = pRquest;
            StaffId = pStaffId;
        }
    }

    public class AfterChangeStatusDeliveryUpdateDeliveryEvent :
        INotificationHandler<AfterChangeStatusDeliveryEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterChangeStatusDeliveryUpdateDeliveryEvent> _logger;

        public AfterChangeStatusDeliveryUpdateDeliveryEvent(
            ILogger<AfterChangeStatusDeliveryUpdateDeliveryEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterChangeStatusDeliveryEvent notification, CancellationToken cancellationToken)
        {
            if(notification.Request.Status == DeliveryStatus.Transport)
            {
                // Cập nhật nhân viên giao hàng và thời gian giao hàng
                var delivery = await _context.Deliveries
                    .FindAsync(notification.Request.DeliveryId);

                delivery.DateSent = DateTime.Now;
                delivery.ShipperId = notification.StaffId;

                _context.Deliveries.Update(delivery);
                await _context.SaveChangesAsync(cancellationToken);

            }
            else if (notification.Request.Status == DeliveryStatus.Delivered)
            {
                // Cập nhật thời gian nhận hàng
                var delivery = await _context.Deliveries
                    .FindAsync(notification.Request.DeliveryId);
                delivery.DateReceipt = DateTime.Now;

                _context.Deliveries.Update(delivery);
                await _context.SaveChangesAsync(cancellationToken);
            }

            await Task.CompletedTask;
        }
    }
}
