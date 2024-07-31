using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.Products.Queries.DetailProduct
{
    public class DetailProductValidator : AbstractValidator<DetailProductCommand>
    {
        public DetailProductValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));

            RuleFor(x => x.Id)
                .MustAsync(async (id, token) =>
                {
                    return await pContext.Products
                        .AnyAsync(x => x.Id == id && x.Type == ProductType.Option) ;
                }).WithMessage("Id sản phẩm không hợp lệ!");
        }
    }
}
