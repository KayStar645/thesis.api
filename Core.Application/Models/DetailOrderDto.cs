namespace Core.Application.Models
{
    public record DetailOrderDto
    {
        public decimal? Cost { get; set; }

        public decimal? ReducedPrice { get; set; }

        public decimal? Price { get; set; }

        public decimal? Profit { get; set; }

        public int? Quantity { get; set; }

        public int? GroupPromotion { get; set; }

        // Sản phẩm
        public int? ProductId { get; set; }

        public ProductDto? Product { get; set; }
    }
}
