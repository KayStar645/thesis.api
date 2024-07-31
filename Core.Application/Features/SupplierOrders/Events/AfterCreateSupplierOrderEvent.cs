using Core.Application.Common.Interfaces;
using Core.Application.Features.SupplierOrders.Commands.CreateSupplierOrder;
using Core.Application.Models;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.SupplierOrders.Events
{
    public class AfterCreateSupplierOrderEvent : INotification
    {
        public CreateSupplierOrderCommand? Request { get; set; }

        public SupplierOrder? Entity { get; set; }

        public SupplierOrderDto? Dto { get; set; }

        public AfterCreateSupplierOrderEvent(CreateSupplierOrderCommand pRequest,
            SupplierOrder? pEntity, SupplierOrderDto pDto)
        {
            Request = pRequest;
            Entity = pEntity;
            Dto = pDto;
        }
    }

    public class AfterCreateSupplierOrderCreateDetailSupplierOrderEvent :
        INotificationHandler<AfterCreateSupplierOrderEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterCreateSupplierOrderCreateDetailSupplierOrderEvent> _logger;

        public AfterCreateSupplierOrderCreateDetailSupplierOrderEvent(
            ILogger<AfterCreateSupplierOrderCreateDetailSupplierOrderEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterCreateSupplierOrderEvent notification, CancellationToken cancellationToken)
        {
            for(int i = 0; i  < notification?.Request?.Details?.LongCount(); i++)
            {
                var detail = new DetailSupplierOrder
                {
                    SupplierOrderId = notification.Entity.Id,
                    ProductId = notification.Request.Details[i].ProductId,
                    Price = notification.Request.Details[i].Price,
                    Quantity = notification.Request.Details[i].Quantity
                };
                await _context.DetailSupplierOrders.AddAsync(detail);
            } 

            await _context.SaveChangesAsync(default(CancellationToken));
            await Task.CompletedTask;
        }
    }
}