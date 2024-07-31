using Core.Application.Common.Interfaces;
using Core.Application.Transforms;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.SupplierOrders.Commands.ChangeStatusSupplierOrder
{
    public class ChangeStatusSupplierOrderValidator : AbstractValidator<ChangeStatusSupplierOrderCommand>
    {
        public ChangeStatusSupplierOrderValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.SupplierOrderId)
                .MustAsync(async (supplierOrderId, token) =>
                {
                    return supplierOrderId == null ||
                           await pContext.SupplierOrders.AnyAsync(x => x.Id == supplierOrderId);
                }).WithMessage(ValidatorTransform.NotExists(Modules.SupplierOrder.SupplierOrderId));

            RuleFor(x => x.Status)
                .MustAsync(async (request, status, token) =>
                {
                    var so = await pContext.SupplierOrders.FindAsync(request.SupplierOrderId);

                    if ((so.Status == SupplierOrderStatus.Draft &&
                        status == SupplierOrderStatus.Order ||
                        status == SupplierOrderStatus.Cancel) ||
                        (so.Status == SupplierOrderStatus.Order &&
                        (status == SupplierOrderStatus.Draft ||
                        status == SupplierOrderStatus.Cancel)))
                    {
                        return true;
                    }

                    return false;
                }).WithMessage("Trạng thái của danh sách sản phẩm nhập không hợp lệ!");
        }
    }
}
