using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Products.Queries.ListPromotionComboProduct
{
    public class ListPromotionComboProductWithPermissionValidator : AbstractValidator<ListPromotionComboProductCommand>
    {
        public ListPromotionComboProductWithPermissionValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
