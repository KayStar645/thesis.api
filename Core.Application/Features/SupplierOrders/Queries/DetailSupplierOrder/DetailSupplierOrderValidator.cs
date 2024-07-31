using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.SupplierOrders.Queries.DetailSupplierOrder
{
    public class DetailSupplierOrderValidator : AbstractValidator<DetailSupplierOrderCommand>
    {
        public DetailSupplierOrderValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));

            RuleFor(x => x.Id)
                .MustAsync(async (id, token) =>
                {
                    return await pContext.SupplierOrders
                    .AnyAsync(x => x.Id == id && x.Type == SupplierOrderType.Order); ;
                }).WithMessage("Id không hợp lệ!");
        }
    }
}
