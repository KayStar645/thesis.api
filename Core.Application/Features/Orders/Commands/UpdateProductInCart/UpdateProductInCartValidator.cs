using Core.Application.Common.Interfaces;
using Core.Application.Transforms;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.Orders.Commands.UpdateProductInCart
{
    public class UpdateProductInCartValidator : AbstractValidator<UpdateProductInCartCommand>
    {
        public UpdateProductInCartValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.ProductId)
                .MustAsync(async (productId, token) =>
                {
                    return await pContext.Products.AnyAsync(x => x.Id == productId && x.Type == ProductType.Option);
                }).WithMessage(ValidatorTransform.NotExists(Modules.Order.ProductId));

            RuleFor(x => x.Quantity)
                .GreaterThanOrEqualTo(Modules.Order.MinQuantity)
                .WithMessage(ValidatorTransform.GreaterThanOrEqualTo(Modules.Order.Quantity, Modules.Order.MinQuantity));
        }
    }
}
