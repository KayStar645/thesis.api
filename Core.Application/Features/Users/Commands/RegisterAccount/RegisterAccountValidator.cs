using Core.Application.Common.Interfaces;
using Core.Application.Common.Validators;
using Core.Application.Features.Users.Commands.BaseUser;
using Core.Application.Transforms;

namespace Core.Application.Features.Users.Commands.RegisterAccount
{
    public class RegisterAccountValidator : AbstractValidator<RegisterAccountCommand>
    {
        public RegisterAccountValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseUserValidator(pContext));

            RuleFor(x => x.Name)
               .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Name))
               .MaximumLength(Modules.NameMax)
               .WithMessage(ValidatorTransform.MaximumLength(Modules.Name, Modules.NameMax));

            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.User.UserName))
                .MinimumLength(Modules.User.UserNameMin)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.User.UserName, Modules.User.UserNameMin))
                .MaximumLength(Modules.User.UserNameMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.User.UserName, Modules.User.UserNameMax))
                .MustAsync(async (userName, token) =>
                {
                    return await pContext.Users.AnyAsync(x => x.UserName == userName) == false;
                }).WithMessage(ValidatorTransform.Exists(Modules.User.UserName));

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.User.Password))
                .MinimumLength(Modules.User.PasswordMin)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.User.Password, Modules.User.PasswordMin))
                .MaximumLength(Modules.User.UserNameMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.User.Password, Modules.User.UserNameMax));

            RuleFor(x => x.Address)
                .MaximumLength(Modules.AddressMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Address, Modules.AddressMax));

            RuleFor(x => x.Gender)
                .Must(gender => ValidatorCustom.IsValidGender(gender))
                .WithMessage(ValidatorTransform.Must(Modules.Gender, ValidatorCustom.GetGender()));
        }
    }
}
