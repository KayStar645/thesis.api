using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Staffs.Queries.ListStaff
{
    public class ListStaffValidator : AbstractValidator<ListStaffCommand>
    {
        public ListStaffValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
