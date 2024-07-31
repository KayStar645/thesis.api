using Core.Domain.Common;
using Sieve.Attributes;

namespace Core.Domain.Entities
{
    public class Coupon : AuditableEntity
    {
        public enum CType
        {
            MC, // Limit người dùng tự đặt, áp dụng cho ai có mã của Coupon này
            SC // Limit 1, áp dụng cho 1 đối tượng khách hàng cụ thể
        }

        public enum CouponType
        {
            Discount,
            Percent,
        }

        public enum CouponStatus
        {
            Draft,
            Approve,
            Cancel
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? InternalCode { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? Name { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? Start { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? End { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int? Limit { get; set; }

        // Giảm giá
        [Sieve(CanFilter = true, CanSort = true)]
        public int? Discount { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int? PercentMax { get; set; }

        // Giảm %
        [Sieve(CanFilter = true, CanSort = true)]
        public int? Percent { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int? DiscountMax { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public CouponType? Type { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public CType? TypeC { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public CouponStatus? Status { get; set; }

        // Khách hàng: Nếu là SC
        [Sieve(CanFilter = true, CanSort = true)]
        public int? CustomerId { get; set; }

        public Customer? Customer { get; set; }

        // Áp dụng cho hoá đơn: Nếu SC: Thực hiện sau khi có hoá đơn


    }
}
