using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Distributors.Queries.ListDistributor
{
    public class ListDistributorValidator : AbstractValidator<ListDistributorCommand>
    {
        public ListDistributorValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
