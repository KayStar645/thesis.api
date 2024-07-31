using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.Customers.Queries.DetailCustomer
{
    public class DetailCustomerValidator : AbstractValidator<GetDetailCustomerCommand>
    {
        public DetailCustomerValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}
