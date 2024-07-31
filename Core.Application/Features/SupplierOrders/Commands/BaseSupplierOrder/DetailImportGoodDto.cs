namespace Core.Application.Features.SupplierOrders.Commands.BaseSupplierOrder
{
    public record DetailImportGoodDto
    {
        public int? Quantity { get; set; }

        public int? ProductId { get; set; }
    }
}
