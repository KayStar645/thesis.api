using Core.Application.Features.Staffs.Commands.BaseStaff;
using Core.Application.Transforms;
using Core.Application.Common.Interfaces;

namespace Core.Application.Features.Staffs.Commands.CreateStaff
{
    public class CreateStaffValidator : AbstractValidator<CreateStaffCommand>
    {
        public CreateStaffValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseStaffValidator(pContext));

            RuleFor(x => x.InternalCode)
               .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.InternalCode))
               .MaximumLength(Modules.InternalCodeMax).WithMessage(ValidatorTransform.MaximumLength(Modules.InternalCode, Modules.InternalCodeMax))
               .MustAsync(async (internalCode, token) =>
               {
                   var exists = await pContext.Staffs
                        .AnyAsync(x => x.InternalCode == internalCode &&
                                       x.IsDeleted == false);
                   return !exists;
               }).WithMessage(ValidatorTransform.Exists(Modules.InternalCode));
        }
    }
}
