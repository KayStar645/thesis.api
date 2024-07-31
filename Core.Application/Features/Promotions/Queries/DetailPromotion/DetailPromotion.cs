using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Promotions.Queries.DetailPromotion
{
    public record DetailPromotionCommand : DetailBaseCommand, IRequest<Result<PromotionDto>>
    {
    }

    public class DetailPromotionCommandHandler :
        DetailBaseCommandHandler<DetailPromotionValidator, DetailPromotionCommand, PromotionDto, Promotion>
    {
        public DetailPromotionCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override async Task<PromotionDto> HandlerDtoAfterQuery(PromotionDto dto)
        {
            dto.PromotionForProduct = await _context.PromotionProductRequirements
                                .Where(x => x.PromotionId == dto.Id)
                                .Include(x => x.Product)
                                .GroupBy(x => x.Group)
                                .Select(x => new PromotionForProductDto
                                {
                                    Group = x.Key,
                                    GroupProducts = x.Select(y => y.ProductId).ToList(),
                                })
                                .ToListAsync();
            return dto;
        }

    }
}
