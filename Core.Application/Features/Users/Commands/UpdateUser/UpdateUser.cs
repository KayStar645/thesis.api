using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Users.Commands.BaseUser;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Auth;

namespace Core.Application.Features.Users.Commands.UpdateUser
{
    public record UpdateUserCommand : UpdateBaseCommand, IBaseUser, IRequest<Result<UserDto>>
    {
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }
    }

    public class UpdateUserCommandHandler :
        UpdateBaseCommandHandler<UpdateUserValidator, UpdateUserCommand, UserDto, User>
    {
        public UpdateUserCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }
    }
}
