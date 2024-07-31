using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Categories.Commands.BaseCategory;

namespace Core.Application.Features.Categories.Commands.UpdatgeCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        public UpdateCategoryValidator(ISupermarketDbContext pContext, int? pCurrentId)
        {
            Include(new UpdateBaseCommandValidator(pContext));
            Include(new BaseCategoryValidator(pContext, pCurrentId));
        }
    }
}
