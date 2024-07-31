namespace Core.Application.Features.Statistical.Queries.StatisticalInventory
{
    public record StatisticalInventoryDto
    {
        public string? Id { get; set; }

        public string? Parent { get; set; }

        public string? Name { get; set; }

        public int? Value { get; set; }
    }
}
