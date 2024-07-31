using Core.Application.Transforms;

namespace Core.Application.Features.Users.Commands.LoginAccount
{
    public class LoginAccountValidator : AbstractValidator<LoginAccountCommand>
    {
        public LoginAccountValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.User.UserName))
                .MinimumLength(Modules.User.UserNameMin)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.User.UserName, Modules.User.UserNameMin))
                .MaximumLength(Modules.User.UserNameMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.User.UserName, Modules.User.UserNameMax));

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.User.Password))
                .MinimumLength(Modules.User.PasswordMin)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.User.Password, Modules.User.PasswordMin))
                .MaximumLength(Modules.User.UserNameMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.User.Password, Modules.User.UserNameMax));
        }
    }
}
