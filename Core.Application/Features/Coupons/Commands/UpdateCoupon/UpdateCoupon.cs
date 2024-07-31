using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Coupons.Commands.BaseCoupon;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using static Core.Domain.Entities.Coupon;

namespace Core.Application.Features.Coupons.Commands.UpdateCoupon
{
    public record UpdateCouponCommand : UpdateBaseCommand, IBaseCoupon, IRequest<Result<CouponDto>>
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

    public class UpdateCouponCommandValidator :
        UpdateBaseCommandHandler<UpdateCouponValidator, UpdateCouponCommand, CouponDto, Coupon>
    {
        public UpdateCouponCommandValidator(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }
    }
}
