using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Users.Queries.GetListUser
{
    public class GetListUserValidator : AbstractValidator<GetListUserCommand>
    {
        public GetListUserValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
