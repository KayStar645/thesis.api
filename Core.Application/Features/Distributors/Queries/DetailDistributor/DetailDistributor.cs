using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Distributors.Queries.DetailDistributor
{
    public record DetailDistributorCommand : DetailBaseCommand, IRequest<Result<DistributorDto>>
    {
    }

    public class DetailDistributorCommandHandler :
        DetailBaseCommandHandler<DetailDistributorValidator, DetailDistributorCommand, DistributorDto, Distributor>
    {
        public DetailDistributorCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

    }
}
