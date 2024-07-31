using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Orders.Queries.ListOrder
{
    public class ListOrderValidator : AbstractValidator<ListOrderCommand>
    {
        public ListOrderValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
