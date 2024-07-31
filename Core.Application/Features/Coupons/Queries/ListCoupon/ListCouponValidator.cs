using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Coupons.Queries.ListCoupon
{
    public class ListCouponValidator : AbstractValidator<ListCouponCommand>
    {
        public ListCouponValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
