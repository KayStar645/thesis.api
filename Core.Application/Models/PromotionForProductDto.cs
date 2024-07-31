namespace Core.Application.Models
{
    public record PromotionForProductDto
    {
        public List<int?>? GroupProducts { get; set; }

        public int? Group { get; set; }
    }
}
