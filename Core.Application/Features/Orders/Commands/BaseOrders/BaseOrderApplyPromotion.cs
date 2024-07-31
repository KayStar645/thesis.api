using Core.Application.Common.Interfaces;
using Core.Domain.Entities;

namespace Core.Application.Features.Orders.Commands.BaseOrders
{
    public static class BaseOrderApplyPromotion
    {
        public static async Task<(Promotion, decimal?, int?)> ApplyPromotionForSingleProduct
            (ISupermarketDbContext pContext, Product? pProduct)
        {
            // Tìm tất cả khuyến mãi của sản phẩm này và chọn cái khuyến mãi cao nhất
            var uniqueGroups = await pContext.PromotionProductRequirements
                    .GroupBy(x => x.Group)
                    .Where(g => g.Count() == 1)
                    .Select(g => g.Key)
                    .ToListAsync();

            var list = await pContext.PromotionProductRequirements
                        .Include(x => x.Promotion)
                        .Where(x => x.ProductId == pProduct.Id &&
                                    x.Promotion.Start <= DateTime.Now && 
                                    DateTime.Now <= x.Promotion.End && 
                                    x.Promotion.Limit >= 1 &&
                                    uniqueGroups.Contains(x.Group) &&
                                    x.Promotion.Status == Promotion.PromotionStatus.Approve)
                        .Select(x => new { x.Promotion, x.Group})
                        .ToListAsync();
            decimal? priceDiscoutMax = 0;
            Promotion promo = null;
            int? group = null;
            if (list != null)
            {
                foreach (var item in list)
                {
                    if (item.Promotion.Type == Promotion.PromotionType.Percent)
                    {
                        decimal? priceDiscout = pProduct.Price * (item.Promotion.Percent * 0.01m) > item.Promotion.DiscountMax ?
                                        item.Promotion.DiscountMax : pProduct.Price * (item.Promotion.Percent * 0.01m);
                        if (priceDiscoutMax < priceDiscout)
                        {
                            priceDiscoutMax = priceDiscout;
                            promo = item.Promotion;
                            group = item.Group;
                        }
                    }
                    else if (item.Promotion.Type == Promotion.PromotionType.Discount)
                    {
                        decimal? priceDiscout = item.Promotion.Discount > pProduct.Price * (item.Promotion.PercentMax * 0.01m) ?
                                        pProduct.Price * (item.Promotion.PercentMax * 0.01m) : item.Promotion.Discount;
                        if (priceDiscoutMax < priceDiscout)
                        {
                            priceDiscoutMax = priceDiscout;
                            promo = item.Promotion;
                            group = item.Group;
                        }
                    }
                }
            }

            return (promo, priceDiscoutMax, group);
        }

    }
}
