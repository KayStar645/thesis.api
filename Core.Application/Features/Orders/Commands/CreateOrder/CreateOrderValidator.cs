using Core.Application.Common.Interfaces;

namespace Core.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator(ISupermarketDbContext pContext, int? pCustomerId)
        {
        }
    }
}
