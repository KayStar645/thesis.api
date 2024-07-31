using Core.Application.Common.Interfaces;
using Core.Application.Features.SupplierOrders.Commands.UpdateSupplierOrder;
using Core.Application.Models;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.SupplierOrders.Events
{
    public class AfterUpdateSupplierOrderEvent : INotification
    {
        public UpdateSupplierOrderCommand? Request { get; set; }

        public SupplierOrder? Entity { get; set; }

        public SupplierOrderDto? Dto { get; set; }

        public AfterUpdateSupplierOrderEvent(UpdateSupplierOrderCommand pRequest,
            SupplierOrder? pEntity, SupplierOrderDto pDto)
        {
            Request = pRequest;
            Entity = pEntity;
            Dto = pDto;
        }
    }

    public class AfterUpdateSupplierOrderUpdateDetailSupplierOrderEvent :
        INotificationHandler<AfterUpdateSupplierOrderEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterUpdateSupplierOrderUpdateDetailSupplierOrderEvent> _logger;

        public AfterUpdateSupplierOrderUpdateDetailSupplierOrderEvent(
            ILogger<AfterUpdateSupplierOrderUpdateDetailSupplierOrderEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterUpdateSupplierOrderEvent notification, CancellationToken cancellationToken)
        {
            // Tìm sản phẩm chung và riêng
            var productIdDB = await _context.DetailSupplierOrders
                .Where(x => x.SupplierOrderId == notification.Request.Id)
                .Select(x => x.ProductId)
                .ToListAsync();
            var productIdRequest = notification.Request.Details
                .Select(x => x.ProductId)
                .ToList();

            // Thêm, sửa, xoá chi tiết
            var create = productIdRequest.Except(productIdDB).ToList();
            var update = productIdRequest.Intersect(productIdDB).ToList();
            var delete = productIdDB.Except(productIdRequest).ToList();

            for (int i = 0; i < create?.LongCount(); i++)
            {
                var detail = new DetailSupplierOrder
                {
                    SupplierOrderId = notification.Entity.Id,
                    ProductId = create[i],
                    Price = notification.Request.Details[i].Price,
                    Quantity = notification.Request.Details[i].Quantity
                };
                await _context.DetailSupplierOrders.AddAsync(detail);
            }

            for (int i = 0; i < update?.LongCount(); i++)
            {
                var detail = await _context.DetailSupplierOrders
                    .Where(x => x.ProductId == update[i] &&
                                x.SupplierOrderId == notification.Entity.Id)
                    .SingleOrDefaultAsync();
                detail.Price = notification.Request.Details[i].Price;
                detail.Quantity = notification.Request.Details[i].Quantity;

                _context.DetailSupplierOrders.Update(detail);
            }

            for (int i = 0; i < delete?.LongCount(); i++)
            {
                var detail = await _context.DetailSupplierOrders
                    .Where(x => x.ProductId == delete[i] &&
                                x.SupplierOrderId == notification.Entity.Id)
                    .SingleOrDefaultAsync();
                _context.DetailSupplierOrders.Remove(detail);
            }

            await _context.SaveChangesAsync(default(CancellationToken));
            await Task.CompletedTask;
        }
    }
}