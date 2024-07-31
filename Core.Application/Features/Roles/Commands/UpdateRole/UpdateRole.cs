using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Roles.Commands.BaseRole;
using Core.Application.Features.Roles.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Auth;

namespace Core.Application.Features.Roles.Commands.UpdateRole
{
    public record UpdateRoleCommand : UpdateBaseCommand, IBaseRole, IRequest<Result<RoleDto>>
    {
        public string? Name { get; set; }

        public List<string>? Permissions { get; set; }
    }

    public class UpdateRoleCommandHandler :
        UpdateBaseCommandHandler<UpdateRoleValidator, UpdateRoleCommand, RoleDto, Role>
    {
        public UpdateRoleCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override async Task After(UpdateRoleCommand request, Role entity, RoleDto dto)
        {
            await _mediator.Publish(new AfterUpdateRoleEvent(request));
        }
    }

}
