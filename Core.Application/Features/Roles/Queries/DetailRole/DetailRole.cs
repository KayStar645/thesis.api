using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Auth;

namespace Core.Application.Features.Roles.Queries.DetailRole
{
    public record DetailRoleCommand : DetailBaseCommand, IRequest<Result<RoleDto>>
    {
    }

    public class DetailRoleCommandHandler :
        DetailBaseCommandHandler<DetailRoleValidator, DetailRoleCommand, RoleDto, Role>
    {
        public DetailRoleCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override async Task<RoleDto> HandlerDtoAfterQuery(RoleDto dto)
        {
            dto.Permissions = await _context.RolePermissions
                    .Include(x => x.Permission)
                    .Where(x => x.RoleId == dto.Id)
                    .Select(x => x.Permission.Name)
                    .ToListAsync();
            return dto;
        }

    }
}
