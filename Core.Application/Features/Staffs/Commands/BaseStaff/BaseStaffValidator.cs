using Core.Application.Common.Interfaces;
using Core.Application.Common.Validators;
using Core.Application.Transforms;

namespace Core.Application.Features.Staffs.Commands.BaseStaff
{
    public class BaseStaffValidator : AbstractValidator<IBaseStaff>
    {
        public BaseStaffValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Name))
                .MaximumLength(Modules.NameMax).WithMessage(ValidatorTransform.MaximumLength(Modules.Name, Modules.NameMax));

            RuleFor(x => x.DateOfBirth)
                .Must(dateOfBirth => string.IsNullOrEmpty(dateOfBirth.ToString()) ||
                                    ValidatorCustom.IsAtLeastNYearsOld(dateOfBirth, Modules.Staff.Year))
                .WithMessage(ValidatorTransform.MustDate(Modules.DateOfBirth, Modules.Staff.Year));

            RuleFor(x => x.Gender)
                .Must(gender => ValidatorCustom.IsValidGender(gender))
                .WithMessage(ValidatorTransform.Must(Modules.Gender, ValidatorCustom.GetGender()));

            RuleFor(x => x.Address)
                .MaximumLength(Modules.AddressMax)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Address, Modules.AddressMax));

            RuleFor(x => x.PhoneNumber)
                .Must(phoneNumber => string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length == Modules.PhoneNumberLength)
                .WithMessage(ValidatorTransform.Length(Modules.PhoneNumber, Modules.PhoneNumberLength));

            RuleFor(x => x.Email)
                .Must(email => string.IsNullOrEmpty(email) || ValidatorCustom.BeValidEmail(email))
                .WithMessage(ValidatorTransform.ValidValue(Modules.Email));

            RuleFor(x => x.IdCard)
               .Must(idCard => string.IsNullOrEmpty(idCard) || idCard.Length == Modules.IdCardLength)
               .WithMessage(ValidatorTransform.Length(Modules.IdCard, Modules.IdCardLength));

            RuleFor(x => x.IdCardImage)
               .Must(idCardImage => (string.IsNullOrEmpty(idCardImage?.Front) && string.IsNullOrEmpty(idCardImage?.Back)) ||
                    (ValidatorCustom.BeValidImage(idCardImage?.Front)) && ValidatorCustom.BeValidImage(idCardImage?.Back))
               .WithMessage(ValidatorTransform.MustTypeUrl(Modules.Staff.IdCardImage, Modules.UrlImage));

            RuleFor(x => x.PositionId)
                .MustAsync(async (positionId, token) =>
                {
                    return positionId == null || await pContext.StaffPositions.AnyAsync(x => x.Id == positionId);
                }).WithMessage(ValidatorTransform.NotExists(Modules.Staff.PositionId));
        }
    }
}
