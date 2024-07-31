using Core.Application.Models.Common;
using Core.Application.Models;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.ImportGoods.Queries.DetailImportGood
{
    public record ProductImportGoodDto : BaseDto
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public List<string>? Images { get; set; }

        public decimal? Price { get; set; }

        public int? Quantity { get; set; }

        public string? Describes { get; set; }

        public string? Feature { get; set; }

        public string? Specifications { get; set; }

        public ProductStatus? Status { get; set; }

        public CategoryDto? Category { get; set; }

        public int? OrderQuantity { get; set; }

        public int? ImportQuantity { get; set; }
    }
}
