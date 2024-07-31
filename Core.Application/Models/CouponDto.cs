using Core.Application.Models.Common;
using static Core.Domain.Entities.Coupon;

namespace Core.Application.Models
{
    public record CouponDto : BaseDto
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

        public CouponType? Type { get; set; }

        public CType TypeC { get; set; }

        public CouponStatus Status { get; set; }

        // Khách hàng: Nếu là MC
        public int? CustomerId { get; set; }

        public CustomerDto? Customer { get; set; }

        // Áp dụng cho hoá đơn: Nếu SC: Thực hiện sau khi có hoá đơn
    }
}
