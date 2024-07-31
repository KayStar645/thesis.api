using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.Distributors.Queries.DetailDistributor
{
    public class DetailDistributorValidator : AbstractValidator<DetailDistributorCommand>
    {
        public DetailDistributorValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}
