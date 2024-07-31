using Core.Application.Common.Interfaces;
using Core.Application.Features.SupplierOrders.Commands.BaseSupplierOrder;
using Core.Application.Features.SupplierOrders.Commands.CreateSupplierOrder;

namespace Core.Application.Features.SupplierOrders.Commands.CreateOrderSupplier
{
    public class CreateSupplierOrderValidator : AbstractValidator<CreateSupplierOrderCommand>
    {
        public CreateSupplierOrderValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseSupplierOrderValidator(pContext));
        }
    }
}
