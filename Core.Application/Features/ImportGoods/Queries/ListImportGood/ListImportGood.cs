using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.ImportGoods.Queries.ListImportGood
{
    public record ListImportGoodCommand : ListBaseCommand, IRequest<PaginatedResult<List<SupplierOrderDto>>>
    {
    }

    public class ListImportGoodCommandHandler :
        ListBaseCommandHandler<ListImportGoodValidator, ListImportGoodCommand, SupplierOrderDto, SupplierOrder>
    {
        public ListImportGoodCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

        protected override IQueryable<SupplierOrder> ApplyQuery(ListImportGoodCommand request, IQueryable<SupplierOrder> query)
        {
            query = query.Where(x => x.Type == SupplierOrderType.Receive);

            if(request.IsAllDetail)
            {
                query = query.Include(x => x.Distributor)
                             .Include(x => x.Parent)
                             .Include(x => x.ApproveStaff);
            }

            return query;
        }
    }
}
