using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Distributors.Queries.ListDistributor
{
    public record ListDistributorCommand : ListBaseCommand, IRequest<PaginatedResult<List<DistributorDto>>>
    {
    }

    public class ListDistributorCommandHandler :
        ListBaseCommandHandler<ListDistributorValidator, ListDistributorCommand, DistributorDto, Distributor>
    {
        public ListDistributorCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

    }
}
