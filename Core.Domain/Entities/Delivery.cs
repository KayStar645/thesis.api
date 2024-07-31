using Core.Domain.Common;
using Sieve.Attributes;

namespace Core.Domain.Entities
{
    public class Delivery : AuditableEntity
    {   
        public enum DeliveryStatus
        {
            Prepare, // Chuẩn bị
            Transport, // Vận chuyển
            Delivered, // Đã giao hàng
            Received, // Nhận hàng
            Cancel // Huỷ 
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? InternalCode { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? DateSent { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? From { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? DateReceipt { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? To { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public decimal? TransportFee { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DeliveryStatus? Status { get; set; }

        // Khoá ngoại
        // Nhân viên đóng gói
        [Sieve(CanFilter = true, CanSort = true)]
        public int? PackingStaffId { get; set; }

        public Staff? PackingStaff { get; set; }

        // Nhân viên giao hàng
        [Sieve(CanFilter = true, CanSort = true)]
        public int? ShipperId { get; set; }

        public Staff? Shipper { get; set; }
    }
}
