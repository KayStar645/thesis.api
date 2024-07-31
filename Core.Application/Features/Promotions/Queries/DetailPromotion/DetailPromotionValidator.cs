using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.Promotions.Queries.DetailPromotion
{
    public class DetailPromotionValidator : AbstractValidator<DetailPromotionCommand>
    {
        public DetailPromotionValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}

