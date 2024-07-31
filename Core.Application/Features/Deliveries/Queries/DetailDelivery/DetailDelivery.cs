using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Deliveries.Queries.DetailDelivery
{
    public record DetailDeliveryCommand : DetailBaseCommand, IRequest<Result<DeliveryDto>>
    {
    }

    public class GetDetailDeliveryCommandHandler :
        DetailBaseCommandHandler<DetailDeliveryValidator, DetailDeliveryCommand, DeliveryDto, Delivery>
    {
        public GetDetailDeliveryCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override IQueryable<Delivery> ApplyQuery(DetailDeliveryCommand request, IQueryable<Delivery> query)
        {
            query = query.Include(x => x.Shipper);
            query = query.Include(x => x.PackingStaff);

            return query;
        }

    }
}
