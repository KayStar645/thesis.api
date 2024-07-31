using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.StaffPositions.Queries.ListStaffPosition
{
    public class ListStaffPositionValidator : AbstractValidator<ListStaffPositionCommand>
    {
        public ListStaffPositionValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
