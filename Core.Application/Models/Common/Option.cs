namespace Core.Application.Models.Common
{
    public record Option
    {
        public enum OptionSelect
        {
            String,
            Number,
            Select
        }

        public string? Name { get; set; }

        public OptionSelect? Select { get; set; }

        public List<string>? Value { get; set; }
    }
}
