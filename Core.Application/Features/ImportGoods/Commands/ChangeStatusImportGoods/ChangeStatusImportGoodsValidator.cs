using Core.Application.Common.Interfaces;
using Core.Application.Transforms;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.ImportGoods.Commands.ChangeStatusImportGoods
{
    public class ChangeStatusImportGoodsValidator : AbstractValidator<ChangeStatusImportGoodsCommand>
    {
        public ChangeStatusImportGoodsValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.SupplierOrderId)
                .MustAsync(async (supplierOrderId, token) =>
                {
                    return supplierOrderId == null ||
                           await pContext.SupplierOrders
                           .AnyAsync(x => x.Id == supplierOrderId &&
                                          x.Status == SupplierOrderStatus.Draft);
                }).WithMessage(ValidatorTransform.NotExists(Modules.SupplierOrder.SupplierOrderId));
        }
    }
}
