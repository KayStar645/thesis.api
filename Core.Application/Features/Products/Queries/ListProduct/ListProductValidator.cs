using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Products.Queries.ListProduct
{
    public class ListProductValidator : AbstractValidator<ListProductCommand>
    {
        public ListProductValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
