using Core.Application.Features.Deliveries.Commands.ChangeStatusDelivery;
using Core.Application.Features.Deliveries.Commands.CreateDelivery;
using Core.Application.Features.Deliveries.Commands.UpdateDelivery;
using Core.Application.Features.Deliveries.Queries.DetailDelivery;
using Core.Application.Features.Deliveries.Queries.ListDelivery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DeliveryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách đơn giao hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("delivery.view")]
        public async Task<ActionResult> Get([FromQuery] ListDeliveryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin chi tiết đơn giao hàng theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("delivery.view")]
        public async Task<ActionResult> Get([FromQuery] DetailDeliveryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thêm mới giao hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - From: Giao từ đâu
        /// - To: Giao tới đâu
        /// - TransportFee: Phí vận chuyển
        /// </remarks>
        [HttpPost]
        [Permission("delivery.create")]
        public async Task<ActionResult> Post([FromBody] CreateDeliveryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Sửa thông tin đơn giao hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpPut]
        [Permission("delivery.update")]
        public async Task<ActionResult> Put([FromBody] UpdateDeliveryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thay đổi trạng thái đơn giao hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - DeliveryId: Id của đơn giao hàng
        /// - Status: Trạng thái thay đổi của đơn giao hàng
        /// (0): Prepare (chuẩn bị),
        /// (1): Transport (Vận chuyển)
        /// (2): Delivered (Đã giao hàng)
        /// (3): Received (Nhận hàng)
        /// (4): Cancel (Huỷ)
        /// </remarks>
        [HttpPatch]
        [Permission("delivery.change-status")]
        public async Task<ActionResult> Change([FromBody] ChangeStatusDeliveryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
