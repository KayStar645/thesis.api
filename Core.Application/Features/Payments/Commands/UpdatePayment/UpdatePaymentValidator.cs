using Core.Application.Common.Interfaces;
using Core.Application.Features.Payments.Commands.BasePayment;

namespace Core.Application.Features.Payments.Commands.UpdatePayment
{
    public class UpdatePaymentValidator : AbstractValidator<UpdatePaymentCommand>
    {
        public UpdatePaymentValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            Include(new BasePaymentValidator(pContext, pCurrentId));
        }
    }
}
