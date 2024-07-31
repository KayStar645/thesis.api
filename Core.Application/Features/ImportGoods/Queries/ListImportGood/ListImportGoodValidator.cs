using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.ImportGoods.Queries.ListImportGood
{
    public class ListImportGoodValidator : AbstractValidator<ListImportGoodCommand>
    {
        public ListImportGoodValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
