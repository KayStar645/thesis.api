using Core.Application.Common.Interfaces;
using Core.Application.Features.ImportGoods.Commands.BaseImportGoods;
using Core.Application.Transforms;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.ImportGoods.Commands.CreateImportGoods
{
    public class CreateImportGoodsValidator : AbstractValidator<CreateImportGoodsCommand>
    {
        public CreateImportGoodsValidator(ISupermarketDbContext pContext, int? pSupplierOrderId)
        {
            Include(new BaseImportGoodsValidator(pContext, pSupplierOrderId));

            RuleFor(x => x.SupplierOrderId)
                .MustAsync(async (supplierOrderId, token) =>
                {
                    return supplierOrderId == null ||
                    await pContext.SupplierOrders.AnyAsync(x => x.Id == supplierOrderId &&
                            (x.Status == SupplierOrderStatus.Order ||
                            x.Status == SupplierOrderStatus.PartialReceipt));
                }).WithMessage(ValidatorTransform.NotExists(Modules.SupplierOrder.DetailSupplierOrder));


        }
    }
}
