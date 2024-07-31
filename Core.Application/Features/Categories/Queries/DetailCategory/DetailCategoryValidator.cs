using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.Categories.Queries.DetailCategory
{
    public class DetailCategoryValidator : AbstractValidator<DetailCategoryCommand>
    {
        public DetailCategoryValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}
