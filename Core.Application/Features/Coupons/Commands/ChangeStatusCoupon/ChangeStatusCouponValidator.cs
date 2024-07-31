using Core.Application.Common.Interfaces;
using Core.Application.Features.Coupons.Commands.ChangeStatusCoupon;
using Core.Application.Transforms;
using static Core.Domain.Entities.Coupon;

namespace Core.Application.Features.Coupons.Commands.ChangeStatusCoupon
{
    public class ChangeStatusCouponValidator : AbstractValidator<ChangeStatusCouponCommand>
    {
        public ChangeStatusCouponValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.CouponId)
                   .MustAsync(async (couponId, token) =>
                   {
                       return couponId == null ||
                       await pContext.Coupons.AnyAsync(x => x.Id == couponId && x.IsDeleted == false);
                   }).WithMessage(ValidatorTransform.NotExists(Modules.Coupon.Id));

            var enumValues = Enum.GetValues(typeof(CouponStatus))
                    .Cast<CouponStatus>()
                    .Select(v => v.ToString())
                    .ToArray();

            RuleFor(x => x.Status)
                .IsInEnum()
                .WithMessage(ValidatorTransform.Must(Modules.Coupon.Status, string.Join(", ", enumValues)));
        }
    }
}
