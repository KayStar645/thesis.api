using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.SupplierOrders.Queries.ListSupplierOrder
{
    public class ListSupplierOrderValidator : AbstractValidator<ListSupplierOrderCommand>
    {
        public ListSupplierOrderValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
