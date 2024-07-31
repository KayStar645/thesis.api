using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Categories.Queries.ListCategory
{
    public class ListCategoryValidator : AbstractValidator<ListCategoryCommand>
    {
        public ListCategoryValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
