using Core.Application.Common.Interfaces;
using Core.Application.Features.Products.Commands.BaseProduct;

namespace Core.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseProductValidator(pContext));
        }
    }
}
