using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Roles.Queries.ListRole
{
    public class ListRoleValidator : AbstractValidator<ListRoleCommand>
    {
        public ListRoleValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
