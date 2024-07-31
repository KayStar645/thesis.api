using Core.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;
using static Core.Domain.Entities.Delivery;

namespace Core.Application.Features.Deliveries.Events
{
    public class AfterCreateOrUpdateDeliveryEvent : INotification
    {
        public int? DeliveryId { get; set; }

        public int? OrderId { get; set; }

        public AfterCreateOrUpdateDeliveryEvent(int pDeliveryId, int? pOrderId)
        {
            DeliveryId = pDeliveryId;
            OrderId = pOrderId;
        }
    }

    public class AfterCreateOrUpdateDeliveryUpdateOrderEvent :
        INotificationHandler<AfterCreateOrUpdateDeliveryEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterCreateOrUpdateDeliveryUpdateOrderEvent> _logger;

        public AfterCreateOrUpdateDeliveryUpdateOrderEvent(
            ILogger<AfterCreateOrUpdateDeliveryUpdateOrderEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterCreateOrUpdateDeliveryEvent notification, CancellationToken cancellationToken)
        {
            // Cập nhật lại đơn giao hàng cho đơn hàng
            var order = await _context.Orders.FindAsync(notification.OrderId);
            order.DeliveryId = notification.DeliveryId;

            _context.Orders.Update(order);
            await _context.SaveChangesAsync(cancellationToken);

            await Task.CompletedTask;
        }
    }
}
