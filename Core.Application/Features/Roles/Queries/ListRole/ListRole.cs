using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Auth;
using Sieve.Services;

namespace Core.Application.Features.Roles.Queries.ListRole
{
    public record ListRoleCommand : ListBaseCommand, IRequest<PaginatedResult<List<RoleDto>>>
    {
    }

    public class ListRoleCommandHandler :
        ListBaseCommandHandler<ListRoleValidator, ListRoleCommand, RoleDto, Role>
    {
        public ListRoleCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

    }
}
