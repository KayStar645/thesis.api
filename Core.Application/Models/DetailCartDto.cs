namespace Core.Application.Models
{
    public record DetailCartDto
    {
        public decimal? Cost { get; set; }

        public decimal? ReducedPrice { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public bool? IsSelected { get; set; }

        // Sản phẩm
        public int? ProductId { get; set; }

        public ProductDto? Product { get; set; }
    }
}
