using Core.Application.Common.Interfaces;
using Core.Application.Features.ImportGoods.Commands.CreateImportGoods;
using Core.Application.Models;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.ImportGoods.Events
{
    public class AfterCreateImportGoodEvent : INotification
    {
        public CreateImportGoodsCommand? Request { get; set; }

        public SupplierOrder? Entity { get; set; }

        public ImportGoodDto? Dto { get; set; }

        public AfterCreateImportGoodEvent(CreateImportGoodsCommand pRequest,
            SupplierOrder? pEntity, ImportGoodDto pDto)
        {
            Request = pRequest;
            Entity = pEntity;
            Dto = pDto;
        }
    }

    public class AfterCreateImportGoodCreateDetailImportGoodEvent :
        INotificationHandler<AfterCreateImportGoodEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterCreateImportGoodCreateDetailImportGoodEvent> _logger;

        public AfterCreateImportGoodCreateDetailImportGoodEvent(
            ILogger<AfterCreateImportGoodCreateDetailImportGoodEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterCreateImportGoodEvent notification, CancellationToken cancellationToken)
        {
            for (int i = 0; i < notification?.Request?.Details?.LongCount(); i++)
            {
                var price = await _context.DetailSupplierOrders
                    .Where(x => x.SupplierOrderId == notification.Request.SupplierOrderId &&
                                x.ProductId == notification.Request.Details[i].ProductId)
                    .Select(x => x.Price)
                    .FirstOrDefaultAsync();

                var detail = new DetailSupplierOrder
                {
                    SupplierOrderId = notification.Entity.Id,
                    ProductId = notification.Request.Details[i].ProductId,
                    Price = price,
                    Quantity = notification.Request.Details[i].ImportQuantity
                };
                await _context.DetailSupplierOrders.AddAsync(detail);
            }
            await _context.SaveChangesAsync(default(CancellationToken));
            await Task.CompletedTask;
        }
    }

    public class AfterCreateImportGoodUpdateImportGoodEvent :
        INotificationHandler<AfterCreateImportGoodEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterCreateImportGoodUpdateImportGoodEvent> _logger;

        public AfterCreateImportGoodUpdateImportGoodEvent(
            ILogger<AfterCreateImportGoodUpdateImportGoodEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterCreateImportGoodEvent notification, CancellationToken cancellationToken)
        {
            var supplierOrder = await _context.SupplierOrders.FindAsync(notification.Entity.Id);
            supplierOrder.ParentId = notification?.Request?.SupplierOrderId;
            _context.SupplierOrders.Update(supplierOrder);
            await _context.SaveChangesAsync(default(CancellationToken));

            await Task.CompletedTask;
        }
    }

}