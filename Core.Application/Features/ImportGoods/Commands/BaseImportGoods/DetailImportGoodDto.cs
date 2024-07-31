namespace Core.Application.Features.ImportGoods.Commands.BaseImportGoods
{
    public record DetailImportGoodDto
    {
        public int? ImportQuantity { get; set; }

        public int? ProductId { get; set; }
    }
}
