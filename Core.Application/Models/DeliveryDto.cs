using Core.Application.Models.Common;
using static Core.Domain.Entities.Delivery;

namespace Core.Application.Models
{
    public record DeliveryDto : BaseDto
    {
        public string? InternalCode { get; set; }

        public DateTime? DateSent { get; set; }

        public string? From { get; set; }

        public DateTime? DateReceipt { get; set; }

        public string? To { get; set; }

        public decimal? TransportFee { get; set; }

        public DeliveryStatus? Stutus { get; set; }

        public int? PackingStaffId { get; set; }

        public StaffDto? PackingStaff { get; set; }

        public int? ShipperId { get; set; }

        public StaffDto? Shipper { get; set; }
    }
}
