using Core.Application.Common.Interfaces;
using Core.Application.Features.Roles.Commands.BaseRole;

namespace Core.Application.Features.Roles.Commands.UpdateRole
{
    public class UpdateRoleValidator : AbstractValidator<UpdateRoleCommand>
    {
        public UpdateRoleValidator(ISupermarketDbContext pContext, int? pCurrentId)
        {
            Include(new BaseRoleValidator(pContext, pCurrentId));
        }
    }
}
