using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Orders.Queries.ListOrder
{
    public record ListOrderCommand : ListBaseCommand, IRequest<PaginatedResult<List<OrderDto>>>
    {

    }

    public class ListOrderCommandHandler :
        ListBaseCommandHandler<ListOrderValidator, ListOrderCommand, OrderDto, Order>
    {
        public ListOrderCommandHandler(ISupermarketDbContext pContext, 
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

        protected override IQueryable<Order> ApplyQuery(ListOrderCommand request, IQueryable<Order> query)
        {
            if(_currentUserService.Type == CLAIMS_VALUES.TYPE_USER)
            {
                query = query.Where(x => x.CustomerId == _currentUserService.CustomerId);
            }

            if (request.IsAllDetail)
            {
                query = query
                    .Include(x => x.Payment)
                    .Include(x => x.Customer)
                    .Include(x => x.Delivery)
                    .Include(x => x.StaffApproved);
            }
            query = query
                    .Where(x => x.Status != Order.OrderStatus.Cart);

            return query;
        }

    }
}
