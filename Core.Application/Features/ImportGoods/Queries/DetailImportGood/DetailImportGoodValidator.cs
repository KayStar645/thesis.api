using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.ImportGoods.Queries.DetailImportGood
{
    public class DetailImportGoodValidator : AbstractValidator<DetailImportGoodCommand>
    {
        public DetailImportGoodValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}
