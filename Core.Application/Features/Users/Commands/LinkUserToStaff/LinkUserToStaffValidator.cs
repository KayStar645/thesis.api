using Core.Application.Common.Interfaces;
using Core.Application.Transforms;

namespace Core.Application.Features.Users.Commands.LinkUserToStaff
{
    public class LinkUserToStaffValidator : AbstractValidator<LinkUserToStaffCommand>
    {
        public LinkUserToStaffValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.UserId)
                .MustAsync(async (userId, token) =>
                {
                    return await pContext.Users.FindAsync(userId) != null;
                }).WithMessage(ValidatorTransform.NotExists(Modules.User.Module));

            RuleFor(x => x.StaffId)
                .MustAsync(async (staffId, token) =>
                {
                    return await pContext.Staffs.FindAsync(staffId) != null;
                }).WithMessage(ValidatorTransform.NotExists(Modules.Staff.Module));
        }
    }
}
