using Core.Application.Common.Interfaces;
using Core.Application.Features.ImportGoods.Commands.UpdateImportGoods;
using Core.Application.Models;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.ImportGoods.Events
{
    public class AfterUpdateImportGoodsEvent : INotification
    {
        public UpdateImportGoodsCommand? Request { get; set; }

        public SupplierOrder? Entity { get; set; }

        public ImportGoodDto? Dto { get; set; }

        public AfterUpdateImportGoodsEvent(UpdateImportGoodsCommand pRequest,
            SupplierOrder? pEntity, ImportGoodDto pDto)
        {
            Request = pRequest;
            Entity = pEntity;
            Dto = pDto;
        }
    }

    public class AfterUpdateImportGoodsUpdateDetailSupplierOrderEvent :
        INotificationHandler<AfterUpdateImportGoodsEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterUpdateImportGoodsUpdateDetailSupplierOrderEvent> _logger;

        public AfterUpdateImportGoodsUpdateDetailSupplierOrderEvent(
            ILogger<AfterUpdateImportGoodsUpdateDetailSupplierOrderEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterUpdateImportGoodsEvent notification, CancellationToken cancellationToken)
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
                var price = await _context.DetailSupplierOrders
                    .Include(x => x.SupplierOrder)
                    .Where(x => x.SupplierOrder.ParentId == notification.Request.Id &&
                                x.ProductId == notification.Request.Details[i].ProductId)
                    .Select(x => x.Price)
                    .FirstOrDefaultAsync();

                var detail = new DetailSupplierOrder
                {
                    SupplierOrderId = notification.Entity.Id,
                    ProductId = create[i],
                    Price = price,
                    Quantity = notification.Request.Details[i].ImportQuantity
                };
                await _context.DetailSupplierOrders.AddAsync(detail);
            }

            for (int i = 0; i < update?.LongCount(); i++)
            {
                var price = await _context.DetailSupplierOrders
                    .Include(x => x.SupplierOrder)
                    .Where(x => x.SupplierOrder.ParentId == notification.Request.Id &&
                                x.ProductId == notification.Request.Details[i].ProductId)
                    .Select(x => x.Price)
                    .FirstOrDefaultAsync();

                var detail = await _context.DetailSupplierOrders
                    .Where(x => x.ProductId == update[i] &&
                                x.SupplierOrderId == notification.Entity.Id)
                    .SingleOrDefaultAsync();
                detail.Price = price;
                detail.Quantity = notification.Request.Details[i].ImportQuantity;

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