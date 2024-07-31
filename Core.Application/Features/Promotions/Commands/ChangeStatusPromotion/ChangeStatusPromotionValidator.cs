using Core.Application.Common.Interfaces;
using Core.Application.Transforms;
using static Core.Domain.Entities.Promotion;

namespace Core.Application.Features.Promotions.Commands.ChangeStatusPromotion
{
    public class ChangeStatusPromotionValidator : AbstractValidator<ChangeStatusPromotionCommand>
    {
        public ChangeStatusPromotionValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.PromotionId)
                   .MustAsync(async (promotionId, token) =>
                   {
                       return promotionId == null || 
                       await pContext.Promotions.AnyAsync(x => x.Id == promotionId && x.IsDeleted == false);
                   }).WithMessage(ValidatorTransform.NotExists(Modules.Promotion.Id));

            var enumValues = Enum.GetValues(typeof(PromotionStatus))
                    .Cast<PromotionStatus>()
                    .Select(v => v.ToString())
                    .ToArray();

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(ValidatorTransform.Must(Modules.Promotion.Status, string.Join(", ", enumValues)));
        }
    }
}
