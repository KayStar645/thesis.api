using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.Roles.Queries.DetailRole
{
    public class DetailRoleValidator : AbstractValidator<DetailRoleCommand>
    {
        public DetailRoleValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}
