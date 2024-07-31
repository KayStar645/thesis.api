using Core.Domain.Common;

namespace Core.Domain.Entities
{
    public class PromotionProductRequirement : HardDeleteEntity
    {
        // -1: Single
        // Ngược lại gop cụm và khuyến mãi nếu có đủ
        public int? Group { get; set; }

        public int? PromotionId { get; set; }

        public Promotion? Promotion { get; set; }

        public int? ProductId { get; set; }

        public Product? Product { get; set; }
    }
}
