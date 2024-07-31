using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Payments.Queries.ListPayment
{
    public class ListPaymentValidator : AbstractValidator<ListPaymentCommand>
    {
        public ListPaymentValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
