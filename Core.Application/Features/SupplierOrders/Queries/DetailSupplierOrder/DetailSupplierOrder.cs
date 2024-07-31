using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.SupplierOrders.Queries.DetailSupplierOrder
{
    public record DetailSupplierOrderCommand : DetailBaseCommand, IRequest<Result<SupplierOrderDto>>
    {
    }

    public class DetailSupplierOrderCommandHandler :
        DetailBaseCommandHandler<DetailSupplierOrderValidator, DetailSupplierOrderCommand, SupplierOrderDto, SupplierOrder>
    {
        public DetailSupplierOrderCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override IQueryable<SupplierOrder> ApplyQuery(DetailSupplierOrderCommand request, IQueryable<SupplierOrder> query)
        {
            query = query.Include(x => x.Distributor)
                         .Include(x => x.ApproveStaff);

            return query;
        }

        protected override async Task<SupplierOrderDto> HandlerDtoAfterQuery(SupplierOrderDto dto)
        {
            var details = await _context.DetailSupplierOrders
                .Include(x => x.Product)
                .Where(x => x.SupplierOrderId == dto.Id)
                .ToListAsync();

            dto.Details = _mapper.Map<List<DetailSupplierOrderDto>>(details);

            return dto;
        }
    }
}
