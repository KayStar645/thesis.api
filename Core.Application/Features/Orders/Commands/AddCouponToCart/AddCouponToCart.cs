using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.Design;

namespace Core.Application.Features.Orders.Commands.AddCouponToCart
{
    public record AddCouponToCartCommand : IRequest<Result<bool>>
    {
        public string? InternalCodeCoupon { get; set; }
    }

    public class AddCouponToCartCommandHandler : IRequestHandler<AddCouponToCartCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public AddCouponToCartCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(AddCouponToCartCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new AddCouponToCartValidator(_context, _currentUserService.CustomerId);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                // Kiểm tra nếu không phải người dùng thì không có quyền
                if (_currentUserService.Type != CLAIMS_VALUES.TYPE_USER)
                {
                    return Result<bool>.Failure("Vui lòng đăng ký tài khoản người dùng để đặt hàng!", StatusCodes.Status403Forbidden);
                }

                var cart = await _context.Orders
                    .Where(x => x.CustomerId == _currentUserService.CustomerId &&
                                x.Status == Order.OrderStatus.Cart)
                    .SingleOrDefaultAsync();
                var coupon = await _context.Coupons
                    .Where(x => x.InternalCode == request.InternalCodeCoupon)
                    .SingleOrDefaultAsync();

                // Tính toán tiền khuyến mãi trên đơn hàng
                decimal? priceDiscout = 0;
                if(cart == null)
                {
                    return Result<bool>.Success(false, StatusCodes.Status400BadRequest);
                }
                else
                {
                    if (coupon.Type == Coupon.CouponType.Percent)
                    {
                        priceDiscout = cart.Total * (coupon.Percent * 0.01m) > coupon.DiscountMax ?
                                            coupon.DiscountMax : cart.Total * (coupon.Percent * 0.01m);
                    }
                    else if (coupon.Type == Coupon.CouponType.Discount)
                    {
                        priceDiscout = coupon.Discount > cart.Total * (coupon.PercentMax * 0.01m) ?
                                            cart.Total * (coupon.PercentMax * 0.01m) : coupon.Discount;
                    }
                }

                cart.CouponId = coupon.Id;
                cart.TotalDecrease = priceDiscout;
                cart.TotalAmount = cart.Total - priceDiscout;
                _context.Orders.Update(cart);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(true, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
