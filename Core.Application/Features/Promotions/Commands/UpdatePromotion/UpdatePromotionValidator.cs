using Core.Application.Common.Interfaces;
using Core.Application.Features.Promotions.Commands.BasePromotion;

namespace Core.Application.Features.Promotions.Commands.UpdatePromotion
{
    public class UpdatePromotionValidator : AbstractValidator<UpdatePromotionCommand>
    {
        public UpdatePromotionValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            Include(new BasePromotionValidator(pContext, pCurrentId));
        }
    }
}
