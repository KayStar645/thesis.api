using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Deliveries.Queries.ListDelivery
{
    public record ListDeliveryCommand : ListBaseCommand, IRequest<PaginatedResult<List<DeliveryDto>>>
    {
    }

    public class ListDeliveryCommandHandler :
        ListBaseCommandHandler<ListDeliveryValidator, ListDeliveryCommand, DeliveryDto, Delivery>
    {
        public ListDeliveryCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

        protected override IQueryable<Delivery> ApplyQuery(ListDeliveryCommand request, IQueryable<Delivery> query)
        {
            if (request.IsAllDetail)
            {
                query = query.Include(x => x.PackingStaff);
                query = query.Include(x => x.Shipper);
            }

            return query;
        }

    }
}
