using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetDetailBase;

namespace Core.Application.Features.Deliveries.Queries.DetailDelivery
{
    public class DetailDeliveryValidator : AbstractValidator<DetailDeliveryCommand>
    {
        public DetailDeliveryValidator(ISupermarketDbContext pContext)
        {
            Include(new DetailBaseCommandValidator(pContext));
        }
    }
}
