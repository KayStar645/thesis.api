using Core.Domain.Common;
using Sieve.Attributes;

namespace Core.Domain.Entities
{
    public class Promotion : AuditableEntity
    {
        public enum PromotionType
        {
            Discount,
            Percent,
        }

        public enum PromotionStatus
        {
            Draft,
            Approve,
            Cancel,
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
        public PromotionType? Type { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public PromotionStatus? Status { get; set; }
    }
}
