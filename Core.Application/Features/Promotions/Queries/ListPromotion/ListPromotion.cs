using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Promotions.Queries.ListPromotion
{
    public record ListPromotionCommand : ListBaseCommand, IRequest<PaginatedResult<List<PromotionDto>>>
    {
    }

    public class ListPromotionCommandHandler :
        ListBaseCommandHandler<ListPromotionValidator, ListPromotionCommand, PromotionDto, Promotion>
    {
        public ListPromotionCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

    }
}
