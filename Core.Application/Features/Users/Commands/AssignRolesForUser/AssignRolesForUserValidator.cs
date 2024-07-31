using Core.Application.Common.Interfaces;
using Core.Application.Transforms;

namespace Core.Application.Features.Users.Commands.AssignRolesForUser
{
    public class AssignRolesForUserValidator : AbstractValidator<AssignRolesForUserCommand>
    {
        public AssignRolesForUserValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.UserId)
                .MustAsync(async (userId, token) =>
                {
                    return await pContext.Users.FindAsync(userId) != null;
                }).WithMessage(ValidatorTransform.NotExists(Modules.User.Module));

            RuleFor(x => x.RolesId)
                .MustAsync(async (rolesId, token) =>
                {
                    if(rolesId != null)
                    {
                        foreach (var roleId in rolesId)
                        {
                            var exists = await pContext.Users.FindAsync(roleId) == null;
                            if (exists)
                            {
                                return false;
                            }
                        }
                    }    
                    return true;
                }).WithMessage(ValidatorTransform.NotExists(Modules.Role.Module));
        }
    }
}
