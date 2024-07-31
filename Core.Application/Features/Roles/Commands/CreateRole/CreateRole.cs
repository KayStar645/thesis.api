using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Roles.Commands.BaseRole;
using Core.Application.Features.Roles.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Auth;

namespace Core.Application.Features.Roles.Commands.CreateRole
{
    public record CreateRoleCommand : IBaseRole, IRequest<Result<RoleDto>>
    {
        public string? Name { get; set; }

        public List<string>? Permissions { get; set; }
    }

    public class CreateRoleCommandHandler :
        CreateBaseCommandHandler<CreateRoleValidator, CreateRoleCommand, RoleDto, Role>
    {
        public CreateRoleCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }

        protected override async Task After(CreateRoleCommand request, Role entity, RoleDto dto)
        {
            await _mediator.Publish(new AfterCreateRoleEvent(request));
        }
    }
}
