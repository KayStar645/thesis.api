using Core.Application.Common.Interfaces;
using Core.Application.Features.Coupons.Commands.BaseCoupon;

namespace Core.Application.Features.Coupons.Commands.UpdateCoupon
{
    public class UpdateCouponValidator : AbstractValidator<UpdateCouponCommand>
    {
        public UpdateCouponValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            Include(new BaseCouponValidator(pContext, pCurrentId));
        }
    }
}
