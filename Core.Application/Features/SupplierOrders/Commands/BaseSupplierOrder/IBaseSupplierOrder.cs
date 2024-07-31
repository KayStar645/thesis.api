namespace Core.Application.Features.SupplierOrders.Commands.BaseSupplierOrder
{
    public interface IBaseSupplierOrder
    {
        public int? DistributorId { get; set; }

        public List<DetailSupplierOrderDto>? Details { get; set; }
    }
}
