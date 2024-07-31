using Core.Application.Features.Staffs.Commands.BaseStaff;
using Core.Application.Transforms;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;

namespace Core.Application.Features.Staffs.Commands.UpdateStaff
{
    public class UpdateStaffValidator : AbstractValidator<UpdateStaffCommand>
    {
        public UpdateStaffValidator(ISupermarketDbContext pContext, int? pCurrentId)
        {
            Include(new UpdateBaseCommandValidator(pContext));
            Include(new BaseStaffValidator(pContext));

            RuleFor(x => x.InternalCode)
               .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.InternalCode))
               .MaximumLength(Modules.InternalCodeMax).WithMessage(ValidatorTransform.MaximumLength(Modules.InternalCode, Modules.InternalCodeMax))
               .MustAsync(async (internalCode, token) =>
               {
                   var exists = await pContext.Staffs
                    .FirstOrDefaultAsync(x => x.Id != pCurrentId &&
                                              x.InternalCode == internalCode &&
                                              x.IsDeleted == false);
                   return exists == null;
               }).WithMessage(ValidatorTransform.Exists(Modules.InternalCode));
        }
    }
}
