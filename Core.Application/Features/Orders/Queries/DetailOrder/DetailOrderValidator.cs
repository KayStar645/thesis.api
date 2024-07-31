using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;
using Core.Domain.Entities;

namespace Core.Application.Features.Orders.Queries.DetailOrder
{
    public class DetailOrderValidator : AbstractValidator<DetailOrderCommand>
    {
        public DetailOrderValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));

            RuleFor(x => x.Id)
                .MustAsync(async (id, token) =>
                {
                    return await pContext.Orders
                    .AnyAsync(x => x.Id == id && x.Status != Order.OrderStatus.Cart);
                }).WithMessage("Id của đơn hàng không hợp lệ!");
        }
    }
}
