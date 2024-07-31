using Core.Domain.Common;
using Sieve.Attributes;

namespace Core.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public enum OrderStatus
        {
            Cart,
            Order,
            Approve,
            Transport,
            Received,
            Cancel
        }

        public enum OrderType
        {
            Online,
            Pos,
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? InternalCode { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? Date { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public decimal? TotalAmount { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public decimal? TotalDecrease { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public decimal? Total { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? Message { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public OrderStatus? Status { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public OrderType? Type { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public bool? IsPay { get; set; }

        // Khoá ngoại
        // Phương thức thanh toán
        [Sieve(CanFilter = true, CanSort = true)]
        public int? PaymentId { get; set; }

        public Payment? Payment { get; set; }

        // Áp dụng coupon
        [Sieve(CanFilter = true, CanSort = true)]
        public int? CouponId { get; set; }

        public Coupon? Coupon { get; set; }


        // Khách hàng đặt
        [Sieve(CanFilter = true, CanSort = true)]
        public int? CustomerId { get; set; }

        public Customer? Customer { get; set; }

        // Giao hàng
        [Sieve(CanFilter = true, CanSort = true)]
        public int? DeliveryId { get; set; }

        public Delivery? Delivery { get; set; }

        // Nhân viên duyệt đơn hàng
        [Sieve(CanFilter = true, CanSort = true)]
        public int? StaffApprovedId { get; set; }

        public Staff? StaffApproved { set; get; }
    }
}
