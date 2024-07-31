using Core.Application.Common.Interfaces;
using Core.Application.Features.StaffPositions.Commands.BaseStaffPosition;

namespace Core.Application.Features.StaffPositions.Commands.UpdateStaffPosition
{
    public class UpdateStaffPositionValidator : AbstractValidator<UpdateStaffPositionCommand>
    {
        public UpdateStaffPositionValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            Include(new BaseStaffPositionValidator(pContext, pCurrentId));
        }
    }
}
