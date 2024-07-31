using Core.Application.Common.Interfaces;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Orders.Events
{
    public class BeforeAddProductToCartEvent : INotification
    {
        public int? UserId { get; set; }

        public BeforeAddProductToCartEvent(int? pUserId)
        {
            UserId = pUserId;
        }
    }

    public class BeforeAddProductToCartCreateCartEvent
        : INotificationHandler<BeforeAddProductToCartEvent>
    {

        private readonly ISupermarketDbContext _context;
        private readonly ILogger<BeforeAddProductToCartCreateCartEvent> _logger;

        public BeforeAddProductToCartCreateCartEvent(
            ILogger<BeforeAddProductToCartCreateCartEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(BeforeAddProductToCartEvent notification, CancellationToken cancellationToken)
        {
            // Lấy giỏ hàng của người dùng
            var cart = await _context.Orders
                .Include(x => x.Customer).ThenInclude(x => x.User)
                .Where(x => x.Customer.User.Id == notification.UserId && 
                            x.Status == Order.OrderStatus.Cart)
                .FirstOrDefaultAsync();

            if (cart == null)
            {
                var user = await _context.Users.FindAsync(notification.UserId);

                // Tạo mới giỏ hàng cho người dùng này
                var order = new Order
                {
                    CustomerId = user.CustomerId,
                    TotalAmount = 0,
                    TotalDecrease = 0,
                    Total = 0,
                    Status = Order.OrderStatus.Cart,
                    Type = Order.OrderType.Online,
                };

                var newCart = await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync(cancellationToken);
            }

            await Task.CompletedTask;
        }
    }
}
