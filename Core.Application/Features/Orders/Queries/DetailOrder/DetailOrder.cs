using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Orders.Queries.DetailOrder
{
    public record DetailOrderCommand : DetailBaseCommand, IRequest<Result<OrderDto>>
    {
    }

    public class DetailOrderCommandHandler :
        DetailBaseCommandHandler<DetailOrderValidator, DetailOrderCommand, OrderDto, Order>
    {
        public DetailOrderCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override IQueryable<Order> ApplyQuery(DetailOrderCommand request, IQueryable<Order> query)
        {
            if (_currentUserService.Type == CLAIMS_VALUES.TYPE_USER)
            {
                query = query.Where(x => x.CustomerId == _currentUserService.CustomerId);
            }

            query = query
                    .Include(x => x.Payment)
                    .Include(x => x.Customer)
                    .Include(x => x.Delivery)
                    .Include(x => x.StaffApproved);
            return query;
        }

        protected override async Task<OrderDto> HandlerDtoAfterQuery(OrderDto dto)
        {
            var details = await _context.DetailOrders
                .Include(x => x.Product)
                .Where(x => x.OrderId == dto.Id).ToListAsync();
            dto.Details = _mapper.Map<List<DetailOrderDto>>(details);
            return dto;
        }
    }
}
