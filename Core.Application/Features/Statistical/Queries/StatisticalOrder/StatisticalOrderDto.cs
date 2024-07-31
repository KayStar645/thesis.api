namespace Core.Application.Features.Statistical.Queries.StatisticalOrder
{
    public record StatisticalOrderDto
    {
        public int? Time { get; set; }

        public int? OrderCount { get; set; }
    }
}
