using Core.Application.Common.Interfaces;
using Core.Application.Transforms;

namespace Core.Application.Features.Orders.Commands.RemoveProductInCart
{
    public class RemoveProductInCartCommandValidator : AbstractValidator<RemoveProductInCartCommand>
    {
        public RemoveProductInCartCommandValidator(ISupermarketDbContext pContext, int? pCartId)
        {
            RuleFor(x => x.ProductId)
                .MustAsync(async (productId, token) =>
                {
                    return await pContext.DetailOrders
                            .AnyAsync(x => x.OrderId == pCartId &&
                                           x.ProductId == productId);
                }).WithMessage(ValidatorTransform.NotExists(Modules.Order.ProductId));
        }
    }
}
