using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Features.Roles.Queries.ListRoleWithPermissionWithPermission;

namespace Core.Application.Features.Roles.Queries.ListRoleWithPermission
{
    public class ListRoleWithPermissionValidator : AbstractValidator<ListRoleWithPermissionCommand>
    {
        public ListRoleWithPermissionValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
