using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Auth;
using Sieve.Services;

namespace Core.Application.Features.Users.Queries.GetListUser
{
    public record GetListUserCommand : ListBaseCommand, IRequest<PaginatedResult<List<UserDto>>>
    {
    }

    public class GetListUserCommandHandler :
        ListBaseCommandHandler<GetListUserValidator, GetListUserCommand, UserDto, User>
    {
        public GetListUserCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

        protected override IQueryable<User> ApplyQuery(GetListUserCommand request, IQueryable<User> query)
        {
            if(request.IsAllDetail)
            {
                query = query.Include(x => x.Staff)
                             .Include(x => x.Customer);
            }
            return base.ApplyQuery(request, query);
        }

    }
}
