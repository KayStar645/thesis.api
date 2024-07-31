using Core.Application.Models.Common;

namespace Core.Application.Models
{
    public record PaymentDto : BaseDto
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }
    }
}
