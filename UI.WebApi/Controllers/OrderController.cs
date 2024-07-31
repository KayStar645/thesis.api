using Core.Application.Features.Orders.Commands.AddCouponToCart;
using Core.Application.Features.Orders.Commands.AddProductToCart;
using Core.Application.Features.Orders.Commands.CancelOrder;
using Core.Application.Features.Orders.Commands.ChangeStatusOrder;
using Core.Application.Features.Orders.Commands.CreateOrder;
using Core.Application.Features.Orders.Commands.RemoveProductInCart;
using Core.Application.Features.Orders.Commands.UpdateProductInCart;
using Core.Application.Features.Orders.Queries.DetailCart;
using Core.Application.Features.Orders.Queries.DetailOrder;
using Core.Application.Features.Orders.Queries.ListOrder;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách đơn hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("order.view")]
        public async Task<ActionResult> Get([FromQuery] ListOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin đơn hàng theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("order.view")]
        public async Task<ActionResult> Get([FromQuery] DetailOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin chi tiết giỏ hàng của người dùng
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// </remarks>
        [HttpGet("detail-cart")]
        public async Task<ActionResult> Get([FromQuery] DetailCartCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }


        /// <summary>
        /// Thêm sản phẩm vào giỏ hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - ProductId: Id của sản phẩm
        /// - Quantity: > 0
        /// </remarks>
        [HttpPost("cart")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] AddProductToCartCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Xoá sản phẩm khỏi giỏ hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - ProductId: Id của sản phẩm trong đơn hàng
        /// </remarks>
        [HttpPost("cart-remove")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] RemoveProductInCartCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Cập nhật số lượng của sản phẩm trong giỏ hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - ProductId: Id của sản phẩm
        /// - Quantity: > 0
        /// </remarks>
        [HttpPut("cart")]
        [AllowAnonymous]
        public async Task<ActionResult> Put([FromBody] UpdateProductInCartCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Áp dụng chương trình khuyến mãi
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - InternalCodeCoupon: Mã chương trình khuyến mãi
        /// </remarks>
        [HttpPost("cart-add-coupon")]
        [AllowAnonymous]
        public async Task<ActionResult> AddCoupon([FromBody] AddCouponToCartCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Đặt hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - ProductsId: Id của sản phẩm trong giỏ hàng
        /// - Message: Lời nhắn cho cửa hàng khi đặt hàng
        /// </remarks>
        [HttpPost("order-create")]
        [AllowAnonymous]
        public async Task<ActionResult> Post([FromBody] CreateOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Huỷ đơn đặt hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - OrderId: Id của đơn hàng người dùng
        /// </remarks>
        [HttpPatch("order-cancel")]
        [AllowAnonymous]
        public async Task<ActionResult> Put([FromBody] CancelOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Nhận viên thay đổi trạng thái đơn hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - OrderId: Id của đơn hàng
        /// - Status: Trạng thái thay đổi của đơn hàng
        /// Cart(0), Order(1), Approve(2), Transport(3), Received(4), Cancel(5)
        /// </remarks>
        [HttpPatch("order-change-status")]
        [Permission("order.change-status")]
        public async Task<ActionResult> Change([FromBody] ChangeStatusOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
