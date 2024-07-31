using Core.Application.Common.Interfaces;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Orders.Events
{
    public class AfterUpdateProductInCartEvent : INotification
    {
        public int? CartId { get; set; }

        public AfterUpdateProductInCartEvent(int? pCartId)
        {
            CartId = pCartId;
        }
    }

    public class AfterUpdateProductInCartUpdateCartEvent : INotificationHandler<AfterUpdateProductInCartEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterUpdateProductInCartUpdateCartEvent> _logger;

        public AfterUpdateProductInCartUpdateCartEvent(
            ILogger<AfterUpdateProductInCartUpdateCartEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterUpdateProductInCartEvent notification, CancellationToken cancellationToken)
        {
            var cart = await _context.Orders.FindAsync(notification.CartId);
            
            // Tính tiền trên đơn hàng
            var details = await _context.DetailOrders
                .Where(x => x.OrderId == cart.Id && x.IsSelected == true)
                .ToListAsync();
            cart.TotalAmount = details.Sum(x => x.Price * x.Quantity ?? 0);
            cart.TotalDecrease = 0;
            cart.Total = details.Sum(x => x.Price * x.Quantity ?? 0);

            // Tính khuyến mãi trên đơn hàng
            var coupon = await _context.Coupons.FindAsync(cart.CouponId);

            if(coupon != null)
            {
                decimal? priceDiscout = 0;
                if (coupon.Type == Coupon.CouponType.Percent)
                {
                    priceDiscout = cart.Total * (coupon.Percent * 0.01m) > coupon.DiscountMax ?
                                        coupon.DiscountMax : cart.Total * (coupon.Percent * 0.01m);
                }
                else if (coupon.Type == Coupon.CouponType.Discount)
                {
                    priceDiscout = coupon.Discount > cart.Total * (coupon.PercentMax * 0.01m) ?
                                        cart.Total * (coupon.PercentMax * 0.01m) : coupon.Discount;
                }

                cart.TotalDecrease = priceDiscout;
                cart.TotalAmount = cart.Total - priceDiscout;
            }

            _context.Orders.Update(cart);
            await _context.SaveChangesAsync(cancellationToken);

            await Task.CompletedTask;
        }
    }
}
