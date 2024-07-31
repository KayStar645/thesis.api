using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.Coupons.Queries.DetailCoupon
{
    public class DetailCouponValidator : AbstractValidator<DetailCouponCommand>
    {
        public DetailCouponValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}

