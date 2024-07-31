namespace Core.Application.Features.SupplierOrders.Commands.BaseSupplierOrder
{
    public record DetailSupplierOrderDto
    {
        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public int? ProductId { get; set; }
    }
}
