using Core.Application.Common.Interfaces;
using Core.Domain.Entities;

namespace Core.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderValidator(ISupermarketDbContext pContext, int? pCustomerId)
        {
            RuleFor(x => x.OrderId)
                .MustAsync(async (orderId, token) =>
                {
                    return await pContext.Orders
                            .AnyAsync(x => x.Id == orderId &&
                                        x.CustomerId == pCustomerId &&
                                        x.Status == Order.OrderStatus.Order);
                }).WithMessage("Id đơn hàng không hợp lệ!");
        }
    }
}
