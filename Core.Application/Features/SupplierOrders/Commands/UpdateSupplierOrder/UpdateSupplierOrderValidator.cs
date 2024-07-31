using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.SupplierOrders.Commands.BaseSupplierOrder;
using Core.Application.Transforms;

namespace Core.Application.Features.SupplierOrders.Commands.UpdateSupplierOrder
{
    public class UpdateSupplierOrderValidator : AbstractValidator<UpdateSupplierOrderCommand>
    {
        public UpdateSupplierOrderValidator(ISupermarketDbContext pContext, int? pCurrentId)
        {
            Include(new UpdateBaseCommandValidator(pContext));
            Include(new BaseSupplierOrderValidator(pContext));
        }
    }
}
