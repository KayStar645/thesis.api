using Core.Application.Common.Constants;
using Microsoft.AspNetCore.Authorization;
using UI.WebApi.Constants;
using CONSTANT_CLAIM_TYPES = UI.WebApi.Constants.CONSTANT_CLAIM_TYPES;

namespace UI.WebApi.Middleware
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    public class PermissionRequirementHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Value == CLAIMS_VALUES.TYPE_ADMIN))
            {
                context.Succeed(requirement);
            }
            else if (context.User.HasClaim(c => c.Type == CONSTANT_CLAIM_TYPES.Permission && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }
            else if(CONSTANT_PERMESSION_DEFAULT.PERMISSIONS_NO_LOGIN.Contains(requirement.Permission))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }

}
