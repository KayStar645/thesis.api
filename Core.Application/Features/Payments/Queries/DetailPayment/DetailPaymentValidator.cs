using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.Payments.Queries.DetailPayment
{
    public class DetailPaymentValidator : AbstractValidator<DetailPaymentCommand>
    {
        public DetailPaymentValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}
