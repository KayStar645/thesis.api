namespace Core.Application.Features.ImportGoods.Commands.BaseImportGoods
{
    public interface IBaseImportGoods
	{
        public string? ReceivingStaff { get; set; }

        public List<DetailImportGoodDto>? Details { get; set; }
    }
}
