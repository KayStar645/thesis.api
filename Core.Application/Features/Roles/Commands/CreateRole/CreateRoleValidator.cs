using Core.Application.Common.Interfaces;
using Core.Application.Features.Roles.Commands.BaseRole;

namespace Core.Application.Features.Roles.Commands.CreateRole
{
    public class CreateRoleValidator : AbstractValidator<CreateRoleCommand>
    {
        public CreateRoleValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseRoleValidator(pContext));
        }
    }
}
