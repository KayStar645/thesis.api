namespace Core.Application.Features.Report.Queries.ReportPromotionGroupProfit
{
    public record ReportPromotionGroupProfitDto : ReportDto
    {
        public List<ReportPromotionGroupProfitRow>? Rows { get; set; }
    }

    public record ReportPromotionGroupProfitRow
    {
        public int? Id { get; set; }

        public List<string>? Names { get; set; }

        public int? SellNumber { get; set; }

        public decimal? Revenue { get; set; }

        public decimal? Expense { get; set; }

        public double? Profit { get; set; }
    }
}
