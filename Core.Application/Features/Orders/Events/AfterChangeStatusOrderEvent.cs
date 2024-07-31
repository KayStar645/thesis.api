using Core.Application.Common.Interfaces;
using Core.Application.Features.Orders.Commands.ChangeStatusOrder;
using Microsoft.Extensions.Logging;
using static Core.Domain.Entities.Order;

namespace Core.Application.Features.Orders.Events
{
    public class AfterChangeStatusOrderEvent : INotification
    {
        public ChangeStatusOrderCommand? Request { get; set; }

        public OrderStatus? OldStatus { get; set; }

        public AfterChangeStatusOrderEvent(ChangeStatusOrderCommand pRquest, OrderStatus? pOldStatus)
        {
            Request = pRquest;
            OldStatus = pOldStatus;
        }
    }

    public class AfterChangeStatusOrderUpdateQuantityProductEvent :
        INotificationHandler<AfterChangeStatusOrderEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterChangeStatusOrderUpdateQuantityProductEvent> _logger;

        public AfterChangeStatusOrderUpdateQuantityProductEvent(
            ILogger<AfterChangeStatusOrderUpdateQuantityProductEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterChangeStatusOrderEvent notification, CancellationToken cancellationToken)
        {
            if(notification.OldStatus == OrderStatus.Order &&
               notification.Request.Status == OrderStatus.Approve)
            {
                var order = await _context.Orders.FindAsync(notification.Request.OrderId);

                var details = await _context.DetailOrders
                            .Where(x => x.OrderId == order.Id)
                            .ToListAsync();

                foreach (var item in details)
                {
                    // Cập nhật số lượng sản phẩm
                    var parent = await _context.Products.FindAsync(item.ProductId);
                    parent.Quantity -= item.Quantity;
                    _context.Products.Update(parent);
                    await _context.SaveChangesAsync(cancellationToken);

                    var productsSingle = await _context.Products
                            .Where(x => x.ParentId == item.ProductId)
                            .ToListAsync();
                    // Tính toán lợi nhuận cho từng sản phẩm trong đơn hàng
                    decimal? profit = 0;
                    int? quantity = item.Quantity;
                    foreach (var product in productsSingle)
                    {
                        if (quantity <= product.Quantity)
                        {
                            profit += item.Price * quantity - product.Price * quantity;

                            // Cập nhật lại số lượng sản phẩm trên sp ảo
                            product.Quantity -= quantity;
                            _context.Products.Update(product);
                            await _context.SaveChangesAsync(cancellationToken);

                            break;
                        }
                        profit += item.Price * product.Quantity - product.Price * product.Quantity;
                        quantity -= product.Quantity;

                        // Xoá sản phẩm ảo
                        product.Quantity = 0;
                        _context.Products.Update(product);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    item.Profit = profit;
                    _context.DetailOrders.Update(item);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            await Task.CompletedTask;
        }
    }

    public class AfterChangeStatusOrderUpdateLimitCouponAndPromotionEvent :
        INotificationHandler<AfterChangeStatusOrderEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterChangeStatusOrderUpdateLimitCouponAndPromotionEvent> _logger;

        public AfterChangeStatusOrderUpdateLimitCouponAndPromotionEvent(
            ILogger<AfterChangeStatusOrderUpdateLimitCouponAndPromotionEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterChangeStatusOrderEvent notification, CancellationToken cancellationToken)
        {
            var groups = await _context.DetailOrders
                .GroupBy(x => x.GroupPromotion)
                .Select(x => x.Key)
                .ToListAsync();

            var detailOrders = await _context.DetailOrders
                .Where(x => groups.Contains(x.GroupPromotion) &&
                            x.GroupPromotion != null &&
                            x.OrderId == notification.Request.OrderId)
                .ToListAsync();

            var uniqueDetail = detailOrders.DistinctBy(x => x.GroupPromotion).ToList();

            for (int i = 0; i < uniqueDetail.Count(); i++)
            {
                var promotion = await _context.PromotionProductRequirements
                    .Where(x => x.Group == uniqueDetail[i].GroupPromotion)
                    .Select(x => x.Promotion)
                    .FirstOrDefaultAsync();
                promotion.Limit -= 1;
                _context.Promotions.Update(promotion);
                await _context.SaveChangesAsync(cancellationToken);
            }

            var coupon = await _context.Orders
                    .Include(x => x.Coupon)
                    .Where(x => x.Id == notification.Request.OrderId)
                    .Select(x => x.Coupon)
                    .SingleOrDefaultAsync();
            if(coupon != null)
            {
                coupon.Limit -= 1;
                _context.Coupons.Update(coupon);
                await _context.SaveChangesAsync(cancellationToken);
            }

            await Task.CompletedTask;
        }
    }

    public class AfterChangeStatusOrderUpdateSellingProductEvent :
        INotificationHandler<AfterChangeStatusOrderEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterChangeStatusOrderUpdateQuantityProductEvent> _logger;

        public AfterChangeStatusOrderUpdateSellingProductEvent(
            ILogger<AfterChangeStatusOrderUpdateQuantityProductEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterChangeStatusOrderEvent notification, CancellationToken cancellationToken)
        {
            if (notification.OldStatus == OrderStatus.Order &&
               notification.Request.Status == OrderStatus.Approve)
            {
                var order = await _context.Orders.FindAsync(notification.Request.OrderId);

                var details = await _context.DetailOrders
                            .Where(x => x.OrderId == order.Id)
                            .ToListAsync();

                foreach (var item in details)
                {
                    // Cập nhật số lượng sản phẩm
                    var product = await _context.Products.FindAsync(item.ProductId);
                    product.Selling = product.Selling == null ? item.Quantity :
                                                product.Selling + item.Quantity;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            await Task.CompletedTask;
        }
    }
}
