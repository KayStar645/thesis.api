using Core.Domain.Common;
using Sieve.Attributes;

namespace Core.Domain.Entities
{
    public class DetailOrder : HardDeleteEntity
    {
        [Sieve(CanFilter = true, CanSort = true)]
        public decimal? Cost { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public decimal? ReducedPrice { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public decimal? Price { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public decimal? Profit { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int? Quantity { get; set; }

        [Sieve(CanFilter = true, CanSort = true)]
        public int? GroupPromotion { get; set; }

        // Khoá ngoại
        // Đơn hàng
        [Sieve(CanFilter = true, CanSort = true)]
        public int? OrderId { get; set; }

        public Order? Order { get; set; }

        // Sản phẩm
        [Sieve(CanFilter = true, CanSort = true)]
        public int? ProductId { get; set; }

        public Product? Product { get; set; }

        public bool? IsSelected { get; set; }
    }
}
