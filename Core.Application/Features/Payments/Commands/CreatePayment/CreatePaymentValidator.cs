using Core.Application.Common.Interfaces;
using Core.Application.Features.Payments.Commands.BasePayment;

namespace Core.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentValidator(ISupermarketDbContext pContext)
        {
            Include(new BasePaymentValidator(pContext));
        }
    }
}
