using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Promotions.Commands.BasePromotion;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using static Core.Domain.Entities.Promotion;

namespace Core.Application.Features.Promotions.Commands.UpdatePromotion
{
    public record UpdatePromotionCommand : UpdateBaseCommand, IBasePromotion, IRequest<Result<PromotionDto>>
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

    public class UpdatePromotionCommandValidator :
        UpdateBaseCommandHandler<UpdatePromotionValidator, UpdatePromotionCommand, PromotionDto, Promotion>
    {
        public UpdatePromotionCommandValidator(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }
    }
}
