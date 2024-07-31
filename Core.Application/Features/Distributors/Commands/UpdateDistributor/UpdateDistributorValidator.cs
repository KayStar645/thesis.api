using Core.Application.Common.Interfaces;
using Core.Application.Features.Distributors.Commands.BaseDistributor;

namespace Core.Application.Features.Distributors.Commands.UpdateDistributor
{
    public class UpdateDistributorValidator : AbstractValidator<UpdateDistributorCommand>
    {
        public UpdateDistributorValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            Include(new BaseDistributorValidator(pContext, pCurrentId));
        }
    }
}
