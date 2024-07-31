using Core.Application.Common.Interfaces;
using Core.Application.Features.Deliveries.Commands.BaseDelivery;

namespace Core.Application.Features.Deliveries.Commands.UpdateDelivery
{
    public class UpdateDeliveryValidator : AbstractValidator<UpdateDeliveryCommand>
    {
        public UpdateDeliveryValidator(ISupermarketDbContext pContext)
        {
            Include(new BaseDeliveryValidator(pContext));
        }
    }
}
