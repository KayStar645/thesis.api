using Core.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Orders.Events
{
    public class AfterCreateOrderEvent : INotification
    {
        public int? CartId { get; set; }

        public List<int?>? ProductsId { get; set; }

        public AfterCreateOrderEvent(int? cartId, List<int?>? productsId)
        {
            CartId = cartId;
            ProductsId = productsId;
        }
    }

    public class AfterCreateOrderUpdateCartEvent : INotificationHandler<AfterCreateOrderEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterUpdateProductInCartUpdateCartEvent> _logger;

        public AfterCreateOrderUpdateCartEvent(
            ILogger<AfterUpdateProductInCartUpdateCartEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterCreateOrderEvent notification, CancellationToken cancellationToken)
        {
            var cart = await _context.Orders.FindAsync(notification.CartId);

            int? couponId = cart.CouponId;

            cart.TotalAmount = 0;
            cart.TotalDecrease = 0;
            cart.Total = 0;
            cart.CouponId = null;

            _context.Orders.Update(cart);
            await _context.SaveChangesAsync(cancellationToken);

            if(couponId != null)
            {
                var coupon = await _context.Coupons.FindAsync(couponId);
                coupon.Limit -= 1;
                _context.Coupons.Update(coupon);
                await _context.SaveChangesAsync(cancellationToken);
            }

            await Task.CompletedTask;
        }
    }
}
