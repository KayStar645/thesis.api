using Core.Application.Common.Interfaces;
using Core.Application.Features.ImportGoods.Commands.BaseImportGoods;

namespace Core.Application.Features.ImportGoods.Commands.UpdateImportGoods
{
    public class UpdateImportGoodsValidator : AbstractValidator<UpdateImportGoodsCommand>
    {
        public UpdateImportGoodsValidator(ISupermarketDbContext pContext, int? pSupplierOrderId)
        {
            Include(new BaseImportGoodsValidator(pContext, pSupplierOrderId));
        }
    }
}
