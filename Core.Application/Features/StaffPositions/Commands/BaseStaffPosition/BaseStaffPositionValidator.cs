using Core.Application.Common.Interfaces;
using Core.Application.Transforms;

namespace Core.Application.Features.StaffPositions.Commands.BaseStaffPosition
{
    public class BaseStaffPositionValidator : AbstractValidator<IBaseStaffPosition>
    {
        public BaseStaffPositionValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
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
                        exists = await pContext.StaffPositions
                                    .AnyAsync(x => x.InternalCode == internalCode &&
                                                   x.IsDeleted == false);
                    }
                    else
                    {
                        exists = await pContext.StaffPositions
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
                        exists = await pContext.StaffPositions
                                .AnyAsync(x => x.Name == name &&
                                               x.IsDeleted == false);
                    }
                    else
                    {
                        exists = await pContext.StaffPositions
                                .AnyAsync(x => x.Name == name &&
                                               x.Id != pCurrentId &&
                                               x.IsDeleted == false);
                    }

                    return !exists;
                }).WithMessage(ValidatorTransform.Exists(Modules.Name));

            RuleFor(x => x.Describes)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Describes))
                .MaximumLength(Modules.MaxDescribes)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Describes, Modules.MaxDescribes));

            RuleFor(x => x.Roles)
                .MustAsync(async (roles, token) =>
                {
                    foreach (var role in roles)
                    {
                        var exists = await pContext.Roles.AnyAsync(x => x.Id == role);
                        if(!exists)
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("Id vai trò không hợp lệ!");
        }
    }
}
