using Core.Application.Common.Interfaces;
using Core.Application.Models;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.Coupon;

namespace Core.Application.Features.Coupons.Commands.ChangeStatusCoupon
{
    public record ChangeStatusCouponCommand : IRequest<Result<CouponDto>>
    {
        public int? CouponId { get; set; }

        public CouponStatus? Status { get; set; }
    }

    public class ChangeStatusCouponCommandHandler : IRequestHandler<ChangeStatusCouponCommand, Result<CouponDto>>
    {
        public readonly ISupermarketDbContext _context;
        public readonly IMapper _mapper;

        public ChangeStatusCouponCommandHandler(ISupermarketDbContext pContext, IMapper pMapper)
        {
            _context = pContext;
            _mapper = pMapper;
        }

        public async Task<Result<CouponDto>> Handle(ChangeStatusCouponCommand request, CancellationToken cancellationToken)
        {
            var validator = new ChangeStatusCouponValidator(_context);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<CouponDto>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }

            var findEntity = await _context.Coupons.FindAsync(request.CouponId);

            bool flag1 = findEntity.Status == CouponStatus.Draft &&
                (request.Status == CouponStatus.Approve || request.Status == CouponStatus.Cancel);
            bool flag2 = findEntity.Status == CouponStatus.Approve &&
                (request.Status == CouponStatus.Draft || request.Status == CouponStatus.Cancel);

            if (!flag1 && !flag2)
            {
                return Result<CouponDto>.Failure("Trạng thái không hợp lệ!", StatusCodes.Status400BadRequest);
            }

            findEntity.Status = request.Status;

            var newEntity = _context.Coupons.Update(findEntity);
            await _context.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<CouponDto>(newEntity.Entity);

            return Result<CouponDto>.Success(dto, StatusCodes.Status200OK);
        }
    }
}