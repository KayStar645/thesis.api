using Core.Application.Common.Interfaces;
using Core.Application.Features.Deliveries.Commands.BaseDelivery;

namespace Core.Application.Features.Deliveries.Commands.CreateDelivery
{
    public class CreateDeliveryValidator : AbstractValidator<CreateDeliveryCommand>
    {
        public CreateDeliveryValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseDeliveryValidator(pContext));
        }
    }
}
