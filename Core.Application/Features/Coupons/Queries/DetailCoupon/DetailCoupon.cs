using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Coupons.Queries.DetailCoupon
{
    public record DetailCouponCommand : DetailBaseCommand, IRequest<Result<CouponDto>>
    {
    }

    public class DetailCouponCommandHandler :
        DetailBaseCommandHandler<DetailCouponValidator, DetailCouponCommand, CouponDto, Coupon>
    {
        public DetailCouponCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override IQueryable<Coupon> ApplyQuery(DetailCouponCommand request, IQueryable<Coupon> query)
        {
            if(request.IsAllDetail)
            {
                query = query.Include(x => x.Customer);
            }

            return query;
        }
    }
}
