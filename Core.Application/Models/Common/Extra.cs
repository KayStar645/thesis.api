namespace Core.Application.Models.Common
{
    public record ChildExtra
    {
        public string? Name { get; set; }

        public string? Description { get; set; }
    }

    public record Extra
    {
        public string? Name { get; set; }

        public List<ChildExtra>? Extras;
    }
}
