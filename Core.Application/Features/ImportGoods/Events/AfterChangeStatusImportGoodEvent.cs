using Core.Application.Common.Interfaces;
using Core.Application.Extensions;
using Core.Application.Features.ImportGoods.Commands.ChangeStatusImportGoods;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.ImportGoods.Events
{
    public class AfterChangeStatusImportGoodEvent : INotification
    {
        public ChangeStatusImportGoodsCommand? Request { get; set; }

        public AfterChangeStatusImportGoodEvent(ChangeStatusImportGoodsCommand? pRequest)
        {
            Request = pRequest;
        }
    }

    public class AfterChangeStatusImportGoodUpdateProductEvent :
        INotificationHandler<AfterChangeStatusImportGoodEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterChangeStatusImportGoodUpdateProductEvent> _logger;

        public AfterChangeStatusImportGoodUpdateProductEvent(
            ILogger<AfterChangeStatusImportGoodUpdateProductEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterChangeStatusImportGoodEvent notification, CancellationToken cancellationToken)
        {
            if(notification?.Request?.IsCancel != true)
            {
                var details = await _context.DetailSupplierOrders
                    .Where(x => x.SupplierOrderId == notification.Request.SupplierOrderId)
                    .ToListAsync();

                for (int i = 0; i < details.LongCount(); i++)
                {
                    var product = await _context.Products.FindAsync(details[i].ProductId);
                    product.Quantity += details[i].Quantity;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync(default(CancellationToken));

                    var price = await _context.DetailSupplierOrders
                        .Where(x => x.SupplierOrderId == notification.Request.SupplierOrderId &&
                                    x.ProductId == details[i].ProductId)
                        .Select(x => x.Price)
                        .FirstOrDefaultAsync();

                    var newProduct = new Product();
                    newProduct.CopyPropertiesFrom(product);
                    newProduct.Id = 0;
                    newProduct.ParentId = product.Id;
                    newProduct.Type = Product.ProductType.Single;
                    newProduct.Price = price;
                    newProduct.Quantity = details[i].Quantity;

                    await _context.Products.AddAsync(newProduct);
                    await _context.SaveChangesAsync(default(CancellationToken));
                }
            }

            await Task.CompletedTask;
        }
    }
}
