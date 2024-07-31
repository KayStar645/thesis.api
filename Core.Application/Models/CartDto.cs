using Core.Application.Models.Common;

namespace Core.Application.Models
{
    public record CartDto : BaseDto
    {
        public decimal? TotalAmount { get; set; }

        public decimal? TotalDecrease { get; set; }

        public decimal? Total { get; set; }

        public string? Message { get; set; }

        // Danh sách sản phẩm của giỏ hàng này
        public List<DetailCartDto>? Details { get; set; }
    }
}
