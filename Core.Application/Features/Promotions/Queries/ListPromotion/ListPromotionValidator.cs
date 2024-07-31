using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Promotions.Queries.ListPromotion
{
    public class ListPromotionValidator : AbstractValidator<ListPromotionCommand>
    {
        public ListPromotionValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
