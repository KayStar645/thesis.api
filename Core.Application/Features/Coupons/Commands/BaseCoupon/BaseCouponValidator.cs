using Core.Application.Common.Interfaces;
using Core.Application.Common.Validators;
using Core.Application.Transforms;
using static Core.Domain.Entities.Coupon;

namespace Core.Application.Features.Coupons.Commands.BaseCoupon
{
    public class BaseCouponValidator : AbstractValidator<IBaseCoupon>
    {
        public BaseCouponValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            RuleFor(x => x.InternalCode)
                   .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.InternalCode))
                   .MinimumLength(Modules.InternalCodeMin)
                   .WithMessage(ValidatorTransform.MinimumLength(Modules.InternalCode, Modules.InternalCodeMin))
                   .MaximumLength(Modules.InternalCodeMax)
                   .WithMessage(ValidatorTransform.MinimumLength(Modules.InternalCode, Modules.InternalCodeMax))
                   .MustAsync(async (internalCode, token) =>
                   {
                       bool exists;

                       if (pCurrentId == null)
                       {
                           exists = await pContext.Coupons
                           .AnyAsync(x => x.InternalCode == internalCode &&
                                          x.IsDeleted == false);
                       }
                       else
                       {
                           exists = await pContext.Coupons
                           .AnyAsync(x => x.InternalCode == internalCode &&
                                          x.Id != pCurrentId &&
                                          x.IsDeleted == false);
                       }

                       return !exists;
                   }).WithMessage(ValidatorTransform.Exists(Modules.InternalCode));

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Name))
                .MinimumLength(Modules.NameMin)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.Name, Modules.NameMin))
                .MaximumLength(Modules.NameMax)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.Name, Modules.NameMax))
                .MustAsync(async (name, token) =>
                {
                    bool exists;

                    if (pCurrentId == null)
                    {
                        exists = await pContext.Coupons
                        .AnyAsync(x => x.Name == name &&
                                       x.IsDeleted == false);
                    }
                    else
                    {
                        exists = await pContext.Coupons
                        .AnyAsync(x => x.Name == name &&
                                       x.Id != pCurrentId &&
                                       x.IsDeleted == false);
                    }
                    return !exists;
                }).WithMessage(ValidatorTransform.Exists(Modules.Name));

            RuleFor(x => x.Start)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Coupon.Start))
                .Must(start => ValidatorCustom.IsAfterDay(start, DateTime.Now))
                .WithMessage(ValidatorTransform.GreaterThanDay(Modules.Coupon.Start, DateTime.Now));

            RuleFor(x => x.End)
                 .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Coupon.End))
                 .Must((x, end) => ValidatorCustom.IsAfterDay(end, x.Start))
                 .WithMessage((x, end) => ValidatorTransform.GreaterThanDay(Modules.Coupon.End, (DateTime)x.Start));

            RuleFor(x => x.TypeC)
                .Must((x, typeC) =>
                {
                    if (typeC == CType.MC)
                    {
                        if (x.Limit < 1)
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("Limit - Giới hạn số lượng phiếu khuyến mãi phải lớn hơn hoặc bằng 1!");

            RuleFor(x => x.TypeC)
                .Must((x, typeC) =>
                {
                    if (typeC == CType.SC)
                    {
                        if (x.Limit != 1)
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("Limit - Giới hạn số lượng phiếu khuyến mãi phải bằng 1!");

            RuleFor(x => x.Type)
                .Must((x, type) =>
                {
                    if (type == CouponType.Discount)
                    {
                        if (x.Discount <= 0 || !(0 < x.PercentMax && x.PercentMax <= 100))
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("Discount phải lớn hơn 0 và PercentMax phải từ 1 đến 100!");

            RuleFor(x => x.Type)
                .Must((x, type) =>
                {
                    if (type == CouponType.Percent)
                    {
                        if (x.DiscountMax <= 0 || !(0 < x.Percent && x.Percent <= 100))
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("Percent phải từ 1 đến 100 và DiscountMax phải lớn hơn 0!");

            RuleFor(x => x.TypeC)
                .MustAsync(async (c, typeC, token) =>
                {
                    if (typeC == CType.SC)
                    {
                        return await pContext.Customers.AnyAsync(x => x.Id == c.CustomerId);
                    }
                    return true;
                }).WithMessage("CustomerId - Mã khách hàng không hợp lệ!");
        }
    }
}
