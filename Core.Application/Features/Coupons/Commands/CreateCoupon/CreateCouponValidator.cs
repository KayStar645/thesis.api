using Core.Application.Common.Interfaces;
using Core.Application.Features.Coupons.Commands.BaseCoupon;

namespace Core.Application.Features.Coupons.Commands.CreateCoupon
{
    public class CreateCouponValidator : AbstractValidator<CreateCouponCommand>
    {
        public CreateCouponValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseCouponValidator(pContext));
        }
    }
}
