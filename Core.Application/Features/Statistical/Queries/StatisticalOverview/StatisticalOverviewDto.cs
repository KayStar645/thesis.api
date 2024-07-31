namespace Core.Application.Features.Statistical.Queries.StatisticalOverview
{
    public record StatisticalOverviewDto
    {
        public int? ItemTotal { get; set; }

        public int? CustomerTotal { get; set; }

        public int? OutStock { get; set; }

        public int? OrderTotal { get; set; }
    }
}
