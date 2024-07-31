using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Customers.Queries.ListCustomer
{
    public class ListCustomerValidator : AbstractValidator<ListCustomerCommand>
    {
        public ListCustomerValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
