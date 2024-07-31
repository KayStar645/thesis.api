using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;

namespace Core.Application.Features.Deliveries.Queries.ListDelivery
{
    public class ListDeliveryValidator : AbstractValidator<ListDeliveryCommand>
    {
        public ListDeliveryValidator(ISupermarketDbContext pContext)
        {
            Include(new ListBaseCommandValidator(pContext));
        }
    }
}
