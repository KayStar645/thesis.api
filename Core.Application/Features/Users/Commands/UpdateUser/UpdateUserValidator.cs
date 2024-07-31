using Core.Application.Common.Interfaces;
using Core.Application.Features.Users.Commands.BaseUser;

namespace Core.Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            Include(new BaseUserValidator(pContext, pCurrentId));
        }
    }
}
