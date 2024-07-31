namespace Core.Application.Features.Report
{
    public record ReportDto
    {
        public string? CompanyName { get; set; }

        public string? Name { get; set; }

        public string? Time { get; set; }

        public string? Address { get; set; }

        public decimal? Revenue { get; set; }

        public decimal? Expense { get; set; }

        public double? Profit { get; set; }

        public string? CreateBy { get; set; }
    }
}
