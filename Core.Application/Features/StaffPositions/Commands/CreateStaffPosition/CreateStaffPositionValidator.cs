using Core.Application.Common.Interfaces;
using Core.Application.Features.StaffPositions.Commands.BaseStaffPosition;

namespace Core.Application.Features.StaffPositions.Commands.CreateStaffPosition
{
    public class CreateStaffPositionValidator : AbstractValidator<CreateStaffPositionCommand>
    {
        public CreateStaffPositionValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseStaffPositionValidator(pContext));
        }
    }
}
