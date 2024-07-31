namespace Core.Application.Features.Report.Queries.ReportProfit
{
    public record ReportProfitDto : ReportDto
    {
        public List<ReportProfitRow>? Rows { get; set; }
    }

    public record ReportProfitRow
    {
        public int? Id { get; set; }

        public List<ReportProfitItem>? Items { get; set; }

        public double? GroupProfits { get; set; }
    }

    public record ReportProfitItem
    {
        public string? Name { get; set; }

        public int? SellNumber { get; set; }

        public decimal? Revenue { get; set; }

        public decimal? Expense { get; set; }

        public decimal? Profit { get; set; }

        public bool IsCategory { get; set; }
    }
}
