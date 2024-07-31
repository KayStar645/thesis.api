using Core.Application.Common.Interfaces;
using Core.Application.Transforms;
using static Core.Domain.Entities.Promotion;

namespace Core.Application.Features.Promotions.Commands.ApplyPromotionForProduct
{
    public class ApplyPromotionForProductValidator : AbstractValidator<ApplyPromotionForProductCommand>
    {
        public ApplyPromotionForProductValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.PromotionId)
                   .MustAsync(async (promotionId, token) =>
                   {
                       return promotionId == null ||
                       await pContext.Promotions.AnyAsync(x => x.Id == promotionId &&
                            x.Status == PromotionStatus.Draft);
                   }).WithMessage(ValidatorTransform.NotExists(Modules.Promotion.Id));

            RuleFor(x => x.Group)
                .MustAsync(async (x, group, token) =>
                {
                    // Danh sách sản phẩm null
                    if (x.ProductsId == null)
                    {
                        return false;
                    }

                    if (group != -1)
                    {
                        var exists = await pContext.PromotionProductRequirements
                            .AnyAsync(x => x.Group == group);
                        if (!exists)
                        {
                            return false;
                        }
                    }
                    foreach (var productId in x.ProductsId)
                    {
                        var product = await pContext.Products.FindAsync(productId);
                        if (product == null)
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("Danh sách sản phẩm trong chương trình khuyến mãi không hợp lệ hoặc đã tồn tại!");
        }
    }
}
