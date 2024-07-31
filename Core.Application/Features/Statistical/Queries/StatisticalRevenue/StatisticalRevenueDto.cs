namespace Core.Application.Features.Statistical.Queries.StatisticalRevenue
{
    public record StatisticalRevenueDto
    {
        public int? Time { get; set; }

        public decimal? Revenue { get; set; }
    }
}
