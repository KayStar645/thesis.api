using Core.Application.Models.Common;
using static Core.Domain.Entities.Order;

namespace Core.Application.Models
{
    public record OrderDto : BaseDto
    {
        public string? InternalCode { get; set; }

        public DateTime? Date { get; set; }

        public decimal? TotalAmount { get; set; }

        public decimal? TotalDecrease { get; set; }

        public decimal? Total { get; set; }

        public string? Message { get; set; }

        public OrderStatus? Status { get; set; }

        public OrderType? Type { get; set; }

        public bool? IsPay { get; set; }

        // Danh sách sản phẩm của đơn hàng này
        public List<DetailOrderDto>? Details { get; set; }

        // Phương thức thanh toán
        public int? PaymentId { get; set; }

        public PaymentDto? Payment { get; set; }

        // Khách hàng đặt
        public int? CustomerId { get; set; }

        public CustomerDto? Customer { get; set; }

        // Giao hàng
        public int? DeliveryId { get; set; }

        public DeliveryDto? Delivery { get; set; }

        // Nhân viên duyệt đơn hàng
        public int? StaffApprovedId { get; set; }

        public StaffDto? StaffApproved { set; get; }
    }
}
