using Core.Application.Models.Common;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Models
{
    public record SupplierOrderDto : BaseDto
    {
        public string? InternalCode { get; set; }

        public DateTime? BookingDate { get; set; }

        public decimal? Total { get; set; }

        public string? Deliver { get; set; }

        public SupplierOrderStatus? Status { get; set; }

        // Khoá ngoại

        // Nhân viên nhận hàng: duyệt đơn hàng này
        public int? ApproveStaffId { get; set; }

        public StaffDto? ApproveStaff { get; set; }

        // Nhà cung cấp

        public int? DistributorId { get; set; }

        public DistributorDto? Distributor { get; set; }

        public List<DetailSupplierOrderDto>? Details { get; set; }
    }
}
