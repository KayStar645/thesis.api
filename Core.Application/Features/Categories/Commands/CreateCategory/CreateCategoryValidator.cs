using Core.Application.Common.Interfaces;
using Core.Application.Features.Categories.Commands.BaseCategory;

namespace Core.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseCategoryValidator(pContext));
        }
    }
}
