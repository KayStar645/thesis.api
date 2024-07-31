using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Coupons.Commands.BaseCoupon;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using static Core.Domain.Entities.Coupon;

namespace Core.Application.Features.Coupons.Commands.CreateCoupon
{
    public record CreateCouponCommand : IBaseCoupon, IRequest<Result<CouponDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? End { get; set; }

        public int? Limit { get; set; }

        // Giảm giá
        public int? Discount { get; set; }

        public int? PercentMax { get; set; }

        // Giảm %
        public int? Percent { get; set; }

        public int? DiscountMax { get; set; }

        public CouponType? Type { get; set; }

        public CType TypeC { get; set; }

        // Khách hàng: Nếu là MC
        public int? CustomerId { get; set; }

        // Áp dụng cho hoá đơn: Nếu SC: Thực hiện sau khi có hoá đơn
    }

    public class CreateCouponCommandHandler :
        CreateBaseCommandHandler<CreateCouponValidator, CreateCouponCommand, CouponDto, Coupon>
    {
        public CreateCouponCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }

        protected override async Task<Coupon> Before(CreateCouponCommand request)
        {
            var coupon = _mapper.Map<Coupon>(request);
            coupon.Status = CouponStatus.Draft;

            if (coupon.Type == CouponType.Discount)
            {
                coupon.Percent = 0;
                coupon.DiscountMax = 0;
            }
            if (coupon.Type == CouponType.Percent)
            {
                coupon.Discount = 0;
                coupon.PercentMax = 0;
            }
            if(coupon.TypeC == CType.SC)
            {
                coupon.CustomerId = null;
            }

            return coupon;
        }
    }
}
