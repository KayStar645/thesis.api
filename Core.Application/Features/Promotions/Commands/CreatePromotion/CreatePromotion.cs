using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Promotions.Commands.BasePromotion;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using static Core.Domain.Entities.Promotion;

namespace Core.Application.Features.Promotions.Commands.CreatePromotion
{
    public record CreatePromotionCommand : IBasePromotion, IRequest<Result<PromotionDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public int? Limit { get; set; }

        // Giảm giá
        public int? Discount { get; set; }

        public int? PercentMax { get; set; }

        // Giảm %
        public int? Percent { get; set; }

        public int? DiscountMax { get; set; }

        public PromotionType? Type { get; set; }
    }

    public class CreatePromotionCommandHandler :
        CreateBaseCommandHandler<CreatePromotionValidator, CreatePromotionCommand, PromotionDto, Promotion>
    {
        public CreatePromotionCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }

        protected override async Task<Promotion> Before(CreatePromotionCommand request)
        {
            var promotion = _mapper.Map<Promotion>(request);
            promotion.Status = PromotionStatus.Draft;

            if (promotion.Type == PromotionType.Discount)
            {
                promotion.Percent = 0;
                promotion.DiscountMax = 0;
            }
            if(promotion.Type == PromotionType.Percent)
            {
                promotion.Discount = 0;
                promotion.PercentMax = 0;
            }

            return promotion;
        }
    }
}
