using Core.Application.Common.Interfaces;
using Core.Application.Features.Distributors.Commands.BaseDistributor;

namespace Core.Application.Features.Distributors.Commands.CreateDistributor
{
    public class CreateDistributorValidator : AbstractValidator<CreateDistributorCommand>
    {
        public CreateDistributorValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseDistributorValidator(pContext));
        }
    }
}
