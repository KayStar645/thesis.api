using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Customers.Commands.BaseCustomer;
using Core.Application.Features.Staffs.Commands.BaseStaff;
using Core.Application.Features.Staffs.Commands.UpdateStaff;
using Core.Application.Transforms;

namespace Core.Application.Features.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerCommand>
    {
        public UpdateCustomerValidator(ISupermarketDbContext pContext, int? pCurrentId)
        {
            Include(new UpdateBaseCommandValidator(pContext));
            Include(new BaseCustomerValidator(pContext, pCurrentId));
        }
    }
}
