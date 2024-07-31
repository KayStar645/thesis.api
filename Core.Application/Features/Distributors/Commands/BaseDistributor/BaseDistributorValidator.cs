using Core.Application.Common.Interfaces;
using Core.Application.Extensions;
using Core.Application.Transforms;

namespace Core.Application.Features.Distributors.Commands.BaseDistributor
{
    public class BaseDistributorValidator : AbstractValidator<IBaseDistributor>
    {
        public BaseDistributorValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
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
                        exists = await pContext.Distributors
                                    .AnyAsync(x => x.InternalCode == internalCode &&
                                                   x.IsDeleted == false);
                    }
                    else
                    {
                        exists = await pContext.Distributors
                                    .AnyAsync(x => x.InternalCode == internalCode &&
                                                   x.Id != pCurrentId &&
                                                   x.IsDeleted == false);
                    }

                    return !exists;
                }).WithMessage(ValidatorTransform.Exists(Modules.InternalCode));

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Name))
                .MaximumLength(Modules.NameMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Name, Modules.NameMax));

            RuleFor(x => x.Address)
                .MaximumLength(Modules.AddressMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Address, Modules.AddressMax));

            RuleFor(x => x.Email)
                .Must(email => ValidatorExtension.BeValidEmail(email))
                .WithMessage(ValidatorTransform.ValidValue(Modules.Email))
                .MustAsync(async (email, token) =>
                {
                    bool exists;

                    if (pCurrentId == null)
                    {
                        exists = await pContext.Distributors
                                .AnyAsync(x => x.Email == email &&
                                               x.IsDeleted == false);
                    }
                    else
                    {
                        exists = await pContext.Distributors
                                .AnyAsync(x => x.Email == email &&
                                               x.Id != pCurrentId &&
                                               x.IsDeleted == false);
                    }

                    return !exists;
                }).WithMessage(ValidatorTransform.Exists(Modules.Email));

            RuleFor(x => x.Phone)
                .Must(phone => phone.Length == Modules.PhoneNumberLength)
                .WithMessage(ValidatorTransform.Length(Modules.PhoneNumber, Modules.PhoneNumberLength))
                .MustAsync(async (phone, token) =>
                {
                    bool exists = true;

                    if (pCurrentId == null)
                    {
                        exists = await pContext.Distributors
                                .AnyAsync(x => x.Phone == phone &&
                                               x.IsDeleted == false);
                    }
                    else
                    {
                        exists = await pContext.Distributors
                                .AnyAsync(x => x.Phone == phone &&
                                               x.Id != pCurrentId &&
                                               x.IsDeleted == false);
                    }

                    return !exists;
                }).WithMessage(ValidatorTransform.Exists(Modules.PhoneNumber));
        }
    }
}
