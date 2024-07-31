using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Coupons.Queries.ListCoupon
{
    public record ListCouponCommand : ListBaseCommand, IRequest<PaginatedResult<List<CouponDto>>>
    {
    }

    public class ListCouponCommandHandler :
        ListBaseCommandHandler<ListCouponValidator, ListCouponCommand, CouponDto, Coupon>
    {
        public ListCouponCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

        protected override IQueryable<Coupon> ApplyQuery(ListCouponCommand request, IQueryable<Coupon> query)
        {
            if (request.IsAllDetail)
            {
                query = query.Include(x => x.Customer);
            }

            if (_currentUserService.Type == CLAIMS_VALUES.TYPE_USER)
            {
                query = query
                    .Where(x => x.CustomerId == _currentUserService.CustomerId ||
                                x.CustomerId == null);
            }

            return query;
        }
    }
}
