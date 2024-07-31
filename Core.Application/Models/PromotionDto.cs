using Core.Application.Models.Common;
using static Core.Domain.Entities.Promotion;

namespace Core.Application.Models
{
    public record PromotionDto : BaseDto
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

        public PromotionStatus? Status { get; set; }

        public List<PromotionForProductDto>? PromotionForProduct { get; set; }
    }
}
