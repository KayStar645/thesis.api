using Core.Application.Common.Interfaces;
using Core.Application.Transforms;

namespace Core.Application.Features.Payments.Commands.BasePayment
{
    public class BasePaymentValidator : AbstractValidator<IBasePayment>
    {
        public BasePaymentValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            RuleFor(x => x.InternalCode)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.InternalCode))
                .MaximumLength(Modules.InternalCodeMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Name, Modules.InternalCodeMax))
                .MustAsync(async (internalCode, token) =>
                {
                    bool exists;

                    if (pCurrentId == null)
                    {
                        exists = await pContext.Payments
                                .AnyAsync(x => x.InternalCode == internalCode &&
                                               x.IsDeleted == false);
                    }
                    else
                    {
                        exists = await pContext.Payments
                                .AnyAsync(x => x.InternalCode == internalCode &&
                                               x.Id != pCurrentId &&
                                               x.IsDeleted == false);
                    }

                    return !exists;
                }).WithMessage(ValidatorTransform.Exists(Modules.InternalCode));

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.InternalCode))
                .MaximumLength(Modules.NameMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Name, Modules.NameMax))
                .MustAsync(async (name, token) =>
                {
                    bool exists;

                    if (pCurrentId == null)
                    {
                        exists = await pContext.Payments
                                .AnyAsync(x => x.Name == name &&
                                               x.IsDeleted == false);
                    }
                    else
                    {
                        exists = await pContext.Payments
                                .AnyAsync(x => x.Name == name &&
                                               x.Id != pCurrentId &&
                                               x.IsDeleted == false);
                    }

                    return !exists;
                }).WithMessage(ValidatorTransform.Exists(Modules.Name));
        }
    }
}
