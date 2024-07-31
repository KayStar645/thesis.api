using Core.Domain.Common;
using Sieve.Attributes;

namespace Core.Domain.Entities
{
    public class SupplierOrder : AuditableEntity
    {
        public enum SupplierOrderType
        {
            Order,
            Receive,
        }

        public enum SupplierOrderStatus
        {
            Draft,
            Order,
            Cancel,
            Completed,
            PartialReceipt,
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? InternalCode { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public DateTime? BookingDate { get; set; }

        public decimal? Total { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? Deliver { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public SupplierOrderType? Type { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public SupplierOrderStatus? Status { get; set; }

        // Khoá ngoại
        public int? ParentId { get; set; }
        public SupplierOrder? Parent { get; set; }


        [Sieve(CanFilter = true, CanSort = true)]
        public string? ReceivingStaff { get; set; }


        [Sieve(CanFilter = true, CanSort = true)]
        public int? ApproveStaffId { get; set; }
        public Staff? ApproveStaff { get; set; }


        [Sieve(CanFilter = true, CanSort = true)]
        public int? DistributorId { get; set; }
        public Distributor? Distributor { get; set; } 
    }
}
