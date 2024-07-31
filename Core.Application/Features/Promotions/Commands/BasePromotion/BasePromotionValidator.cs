using Core.Application.Common.Interfaces;
using Core.Application.Common.Validators;
using Core.Application.Transforms;
using static Core.Domain.Entities.Promotion;

namespace Core.Application.Features.Promotions.Commands.BasePromotion
{
    public class BasePromotionValidator : AbstractValidator<IBasePromotion>
    {
        public BasePromotionValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
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
                           exists = await pContext.Promotions
                                .AnyAsync(x => x.InternalCode == internalCode &&
                                               x.IsDeleted == false);
                       }
                       else
                       {
                           exists = await pContext.Promotions
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
                        exists = await pContext.Promotions
                                    .AnyAsync(x => x.Name == name &&
                                                   x.IsDeleted == false);
                    }
                    else
                    {
                        exists = await pContext.Promotions
                                    .AnyAsync(x => x.Name == name &&
                                                   x.Id != pCurrentId &&
                                                   x.IsDeleted == false);
                    }
                    return !exists;
                }).WithMessage(ValidatorTransform.Exists(Modules.Name));

            RuleFor(x => x.Start)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Promotion.Start))
                .Must(start => ValidatorCustom.IsAfterDay(start, DateTime.Now))
                .WithMessage(ValidatorTransform.GreaterThanDay(Modules.Promotion.Start, DateTime.Now));

            RuleFor(x => x.End)
                 .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Promotion.End))
                 .Must((x, end) => ValidatorCustom.IsAfterDay(end, x.Start))
                 .WithMessage((x, end) => ValidatorTransform.GreaterThanDay(Modules.Promotion.End, (DateTime)x.Start));

            RuleFor(x => x.Limit)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Promotion.Limit))
                .GreaterThanOrEqualTo(Modules.Promotion.MinLimit)
                .WithMessage(ValidatorTransform.GreaterThanOrEqualTo(Modules.Promotion.Limit, Modules.Promotion.MinLimit));

            RuleFor(x => x.Type)
                .Must((x, type) =>
                {
                    if(type == PromotionType.Discount)
                    {
                        if(x.Discount <= 0 || !(0 < x.PercentMax && x.PercentMax <= 100))
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("Discount phải lớn hơn 0 và PercentMax phải từ 1 đến 100!");

            RuleFor(x => x.Type)
                .Must((x, type) =>
                {
                    if (type == PromotionType.Percent)
                    {
                        if (x.DiscountMax <= 0 || !(0 < x.Percent && x.Percent <= 100))
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("Percent phải từ 1 đến 100 và DiscountMax phải lớn hơn 0!");
        }
    }
}
