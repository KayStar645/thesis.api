using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.SupplierOrders.Queries.ListSupplierOrder
{
    public record ListSupplierOrderCommand : ListBaseCommand, IRequest<PaginatedResult<List<SupplierOrderDto>>>
    {
    }

    public class ListSupplierOrderCommandHandler :
        ListBaseCommandHandler<ListSupplierOrderValidator, ListSupplierOrderCommand, SupplierOrderDto, SupplierOrder>
    {
        public ListSupplierOrderCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

        protected override IQueryable<SupplierOrder> ApplyQuery(ListSupplierOrderCommand request, IQueryable<SupplierOrder> query)
        {
            query = query.Where(x => x.Type == SupplierOrderType.Order);

            if (request.IsAllDetail)
            {
                query = query.Include(x => x.Distributor)
                             .Include(x => x.ApproveStaff);
            }

            return query;
        }

    }
}
