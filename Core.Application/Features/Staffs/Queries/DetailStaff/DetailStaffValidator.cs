using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.Staffs.Queries.DetailStaff
{
    public class DetailStaffValidator : AbstractValidator<DetailStaffCommand>
    {
        public DetailStaffValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}
