using Core.Application.Features.SupplierOrders.Commands.ChangeStatusSupplierOrder;
using Core.Application.Features.SupplierOrders.Commands.CreateSupplierOrder;
using Core.Application.Features.SupplierOrders.Commands.UpdateSupplierOrder;
using Core.Application.Features.SupplierOrders.Queries.DetailSupplierOrder;
using Core.Application.Features.SupplierOrders.Queries.ListSupplierOrder;
using Core.Application.Features.SupplierOrders.Queries.ProductSupplierOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/supplier-order")]
    [ApiController]
    public class SupplierOrderController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupplierOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm yêu cầu nhập
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("supplier-order.view")]
        public async Task<ActionResult> Get([FromQuery] ListSupplierOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin danh sách sản phẩm yêu cầu nhập
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("supplier-order.view")]
        public async Task<ActionResult> Get([FromQuery] DetailSupplierOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy danh sách sản phẩm theo id đơn nhập
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("list-product")]
        [Permission("supplier-order.view")]
        public async Task<ActionResult> GetProduct([FromQuery] ProductSupplierOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Tạo danh sách sản phẩm đặt hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - ReceivingStaffId: Id người nhận hàng từ bảng Staff
        /// - DistributorId: Id nhà cung cấp từ Distributor
        /// - Details: Danh sách sản phẩm
        ///     + ProductId: Id sản phẩm từ bảng Product
        ///     + Quantity: > 0
        ///     + Price: > 0
        /// </remarks>
        [HttpPost]
        [Permission("supplier-order.create")]
        public async Task<ActionResult> Post([FromBody] CreateSupplierOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Cập nhật danh sách sản phẩm đặt hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - ReceivingStaffId: Id người nhận hàng từ bảng Staff
        /// - DistributorId: Id nhà cung cấp từ Distributor
        /// - Details: Danh sách sản phẩm
        ///     + ProductId: Id sản phẩm từ bảng Product
        ///     + Quantity: > 0
        ///     + Price: > 0
        /// </remarks>
        [HttpPut]
        [Permission("supplier-order.update")]
        public async Task<ActionResult> Put([FromBody] UpdateSupplierOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Nhận viên thay đổi trạng thái danh sách sản phẩm nhập hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - SupplierOrderId: Id của danh sách sản phẩm nhập
        /// - Status: Trạng thái thay đổi của ds nhập
        /// Draft(0), Order(1), Cancel(2)
        /// </remarks>
        [HttpPatch("change-status")]
        [Permission("supplier-order.change-status")]
        public async Task<ActionResult> ChangeStatus([FromBody] ChangeStatusSupplierOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
