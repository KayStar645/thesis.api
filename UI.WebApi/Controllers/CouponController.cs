using Core.Application.Features.Coupons.Commands.ChangeStatusCoupon;
using Core.Application.Features.Coupons.Commands.CreateCoupon;
using Core.Application.Features.Coupons.Commands.UpdateCoupon;
using Core.Application.Features.Coupons.Queries.DetailCoupon;
using Core.Application.Features.Coupons.Queries.ListCoupon;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class CouponController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CouponController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách phiếu khuyến mãi
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("coupon.view")]
        public async Task<ActionResult> Get([FromQuery] ListCouponCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin chi tiết phiếu khuyến mãi theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("coupon.view")]
        public async Task<ActionResult> Get([FromQuery] DetailCouponCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thêm mới phiếu khuyến mãi
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - InternalCode: string, required, max(50)
        /// - Name: string, required, max(190)
        /// - Start: > thời gian hiện tại
        /// - End: > thời gian bắt đầu
        /// - Limit: > 0
        /// - Type: Discount (0)
        ///     + Discount: > 0
        ///     + PercentMax: (0, 100]
        /// - Type: Percent (1)
        ///     + Percent: (0, 100]
        ///     + DiscountMax: > 0
        /// - CType: MC(0)
        ///     +
        /// - CType: SC(1)
        ///     + CustomerId: hợp lệ
        /// </remarks>
        [HttpPost]
        [Permission("coupon.create")]
        public async Task<ActionResult> Post([FromBody] CreateCouponCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Sửa thông tin phiếu khuyến mãi
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// - Thông tin chỉnh sửa tương tự thông tin nhập
        /// </remarks>
        [HttpPut]
        [Permission("coupon.update")]
        public async Task<ActionResult> Put([FromBody] UpdateCouponCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thay đổi trạng thái phiếu khuyến mãi
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - CouponId: int, required
        /// - status:
        ///     + Nháp(0) => Duyệt(1)/Huỷ bỏ(2)
        ///     + Duyệt(1) => Nháp(0)/Huỷ bỏ(2)
        ///     + Huỷ bỏ(2) => Không được thay đổi
        /// </remarks>
        [HttpPatch]
        [Permission("coupon.change-status")]
        public async Task<ActionResult> ChangeStatus([FromBody] ChangeStatusCouponCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
