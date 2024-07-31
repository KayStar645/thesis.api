using Core.Application.Common.Interfaces;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Orders.Events
{
    public class AfterAddProductToCartEvent : INotification
    {
        public int? CustomerId { get; set; }

        public AfterAddProductToCartEvent(int? pCustomerId)
        {
            CustomerId = pCustomerId;
        }
    }

    public class AfterAddProductToCartUpdatePromotionGroupEvent :
        INotificationHandler<AfterAddProductToCartEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterAddProductToCartUpdatePromotionGroupEvent> _logger;

        public AfterAddProductToCartUpdatePromotionGroupEvent(
            ILogger<AfterAddProductToCartUpdatePromotionGroupEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterAddProductToCartEvent notification, CancellationToken cancellationToken)
        {
            // Lấy sản phẩm trong giỏ hàng được chọn ra
            var detail = await _context.DetailOrders
                        .Include(x => x.Order)
                        .Where(x => x.Order.CustomerId == notification.CustomerId &&
                                    x.Order.Status == Order.OrderStatus.Cart)
                        .ToListAsync();
            var productsId = detail.Select(x => x.ProductId).ToList();

            // Lấy tất cả CTKM theo nhóm ra
            var groupsG = await _context.PromotionProductRequirements
                .Where(x => x.Promotion.Start <= DateTime.Now &&
                            DateTime.Now <= x.Promotion.End &&
                            x.Promotion.Limit >= 1 &&
                            x.Promotion.Status == Promotion.PromotionStatus.Approve)
                .GroupBy(x => x.Group)
                .ToListAsync();

            var uniqueGroups = groupsG.Where(group => group.Count() > 1).ToList();
            var groups = uniqueGroups.Where(x => x.All(g => productsId.Contains(g.ProductId))).ToList();

            if(groups.Count() == 0)
            {
                return;
            }
            List<int?> uniqueGroup = new List<int?>();
            if (groups.Count() == 1)
            {
                uniqueGroup.Add(groups[0].Key);
            }
            else
            {
                for (int i = 0; i < groups.Count - 1; i++)
                {
                    for (int j = i + 1; j < groups.Count; j++)
                    {
                        var productI = groups[i].Select(x => x.ProductId).ToList();
                        var productJ = groups[j].Select(x => x.ProductId).ToList();
                        bool haveCommonProduct = productI.Intersect(productJ).Any();
                        if (haveCommonProduct)
                        {
                            var promotionI = await _context.PromotionProductRequirements
                                .Include(x => x.Promotion)
                                .Where(x => x.Group == groups[i].Key)
                                .Select(x => x.Promotion)
                                .FirstOrDefaultAsync();

                            var promotionJ = await _context.PromotionProductRequirements
                                .Include(x => x.Promotion)
                                .Where(x => x.Group == groups[j].Key)
                                .Select(x => x.Promotion)
                                .FirstOrDefaultAsync();

                            // Nhiều sp hơn chưa chắc km hơn
                            int? groupAdd = productI.Count > productJ.Count ?
                                                    groups[i].Key : groups[j].Key;
                            uniqueGroup.Add(groupAdd);
                        }
                    }
                }
            }    

            foreach (var group in uniqueGroup)
            {
                var promotionDetail = await _context.PromotionProductRequirements
                    .Include(x => x.Promotion)
                    .Include(x => x.Product)
                    .Where(x => x.Group == group)
                    .ToListAsync();
                decimal? number = promotionDetail.Count();
                foreach (var item in promotionDetail)
                {
                    var detailOrder = detail
                            .Where(x => x.ProductId == item.Product.Id)
                            .SingleOrDefault();
                    if (item.Promotion.Type == Promotion.PromotionType.Percent)
                    {
                        decimal? priceDiscout = item.Product.Price * (item.Promotion.Percent * 0.01m) > item.Promotion.DiscountMax ?
                                        item.Promotion.DiscountMax : item.Product.Price * (item.Promotion.Percent * 0.01m);
                        detailOrder.ReducedPrice = priceDiscout;
                        detailOrder.Price = item.Product.Price - priceDiscout;
                        detailOrder.GroupPromotion = group;
                    }
                    else if (item.Promotion.Type == Promotion.PromotionType.Discount)
                    {
                        decimal? priceDiscout = (item.Promotion.Discount / number) > item.Product.Price * (item.Promotion.PercentMax * 0.01m) ?
                                        item.Product.Price * (item.Promotion.PercentMax * 0.01m) : (item.Promotion.Discount / number);
                        detailOrder.ReducedPrice = priceDiscout;
                        detailOrder.Price = item.Product.Price - priceDiscout;
                        detailOrder.GroupPromotion = group;
                    }
                    _context.DetailOrders.Update(detailOrder);
                    await _context.SaveChangesAsync(cancellationToken);
                }    
            }

            await Task.CompletedTask;
        }
    }
}
