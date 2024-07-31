using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.StaffPositions.Queries.DetailStaffPosition
{
    public class DetailStaffPositionValidator : AbstractValidator<DetailStaffPositionCommand>
    {
        public DetailStaffPositionValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}
