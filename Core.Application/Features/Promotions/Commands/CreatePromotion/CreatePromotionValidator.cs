using Core.Application.Common.Interfaces;
using Core.Application.Features.Promotions.Commands.BasePromotion;

namespace Core.Application.Features.Promotions.Commands.CreatePromotion
{
    public class CreatePromotionValidator : AbstractValidator<CreatePromotionCommand>
    {
        public CreatePromotionValidator(ISupermarketDbContext pContext)
        {
            Include(new BasePromotionValidator(pContext));
        }
    }
}
