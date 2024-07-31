using Core.Domain.Common;
using Sieve.Attributes;

namespace Core.Domain.Entities
{
    public class Product : AuditableEntity
    {
        public enum ProductType
        {
            Option,
            Single,
        }

        public enum ProductStatus
        {
            Draft,
            Active,
            Pause,
            OutStock,
            Stop
        }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? InternalCode { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public string? Name { get; set; }

        public string? Images { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public decimal? Price { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int? Quantity { get; set; }

        public string? Describes { get; set; }
        
        public string? Feature { get; set; }
        
        public string? Specifications { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public ProductType? Type { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public ProductStatus? Status { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int? Selling { get; set; }

        // Khoá ngoại
        [Sieve(CanFilter = true, CanSort = true)]
        public int? ParentId { get; set; }
        public Product? Parent { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
