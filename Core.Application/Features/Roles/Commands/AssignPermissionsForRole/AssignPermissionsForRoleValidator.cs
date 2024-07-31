using Core.Application.Common.Interfaces;
using Core.Application.Transforms;

namespace Core.Application.Features.Roles.Commands.AssignPermissionsForRole
{
    public class AssignPermissionsForRoleValidator : AbstractValidator<AssignPermissionsForRoleCommand>
    {
        public AssignPermissionsForRoleValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.RoleId)
                .MustAsync(async (userId, token) =>
                {
                    return await pContext.Roles.FindAsync(userId) != null;
                }).WithMessage(ValidatorTransform.NotExists(Modules.Role.Module));

            RuleFor(x => x.PermissionsName)
                .MustAsync(async (permissionsName, token) =>
                {
                    if (permissionsName != null)
                    {
                        foreach (var permessionName in permissionsName)
                        {
                            var exists = await pContext.Permissions
                                    .FirstOrDefaultAsync(x => x.Name == permessionName) == null;
                            if (exists)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }).WithMessage(ValidatorTransform.NotExists(Modules.Permission.Module));
        }
    }
}
