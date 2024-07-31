using Core.Application.Common.Interfaces;
using Core.Domain.Entities;

namespace Core.Application.Features.Orders.Commands.ChangeStatusOrder
{
    public class ChangeStatusOrderValidator : AbstractValidator<ChangeStatusOrderCommand>
    {
        public ChangeStatusOrderValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.OrderId)
                .MustAsync(async (orderId, token) =>
                {
                    return await pContext.Orders
                            .AnyAsync(x => x.Id == orderId);
                }).WithMessage("Id đơn hàng không hợp lệ!");

            RuleFor(x => x.Status)
                .MustAsync(async (request, status, token) =>
                {
                    var order = await pContext.Orders.FindAsync(request.OrderId);

                    if(order == null)
                    {
                        return false;
                    }

                    if(!((order.Status == Order.OrderStatus.Order && 
                       (status == Order.OrderStatus.Approve ||
                        status == Order.OrderStatus.Cancel)) ||
                        (order.Status == Order.OrderStatus.Approve &&
                       (status == Order.OrderStatus.Transport)) ||
                       (order.Status == Order.OrderStatus.Transport)))
                    {
                        return false;
                    }  

                    return true;
                }).WithMessage("Trạng thái thay đổi đơn hàng không hợp lệ!");
        }
    }
}
