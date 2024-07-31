using Core.Application.Common.Interfaces;
using Core.Domain.Entities;
using MediatR;

namespace Core.Application.Features.Orders.Commands.AddCouponToCart
{
    public class AddCouponToCartValidator : AbstractValidator<AddCouponToCartCommand>
    {
        public AddCouponToCartValidator(ISupermarketDbContext pContext, int? pCustomerId)
        {
            RuleFor(x => x.InternalCodeCoupon)
                .MustAsync(async (code, token) =>
                {
                    return await pContext.Coupons
                    .AnyAsync(x => x.InternalCode == code &&
                                x.Start <= DateTime.Now && DateTime.Now <= x.End &&
                                x.Status == Coupon.CouponStatus.Approve &&
                                x.Limit > 0 &&
                                (x.TypeC == Coupon.CType.MC ||
                                (x.TypeC == Coupon.CType.SC &&
                                x.CustomerId == pCustomerId)));
                }).WithMessage("Mã chương trình khuyến mãi không hợp lệ!");
        }
    }
}
