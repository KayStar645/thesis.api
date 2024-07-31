using Core.Application.Features.ImportGoods.Commands.ChangeStatusImportGoods;
using Core.Application.Features.ImportGoods.Commands.CreateImportGoods;
using Core.Application.Features.ImportGoods.Commands.UpdateImportGoods;
using Core.Application.Features.ImportGoods.Queries.ListImportGood;
using Core.Application.Features.ImportGoods.Queries.DetailImportGood;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/import-goods")]
    [ApiController]
    public class ImportGoodsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImportGoodsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách hoá đơn nhập hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("import-good.view")]
        public async Task<ActionResult> Get([FromQuery] ListImportGoodCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin hoá đơn nhập hàng theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("import-good.view")]
        public async Task<ActionResult> Get([FromQuery] DetailImportGoodCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Nhập hàng từ nhà cung cấp khi họ giao hàng tới - Nháp
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - SupplierOrderId: Id từ đơn đặt hàng nào trong bảng SupplierOrder
        /// - Details: Danh sách sản phẩm
        ///     + ProductId: Id sản phẩm từ bảng Product
        ///     + Quantity: > 0
        /// </remarks>
        [HttpPost]
        [Permission("import-good.create")]
        public async Task<ActionResult> Post([FromBody] CreateImportGoodsCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Cập nhật nhập hàng từ nhà cung cấp khi họ giao hàng tới
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - SupplierOrderId: Id từ đơn đặt hàng nào trong bảng SupplierOrder
        /// - Details: Danh sách sản phẩm
        ///     + ProductId: Id sản phẩm từ bảng Product
        ///     + Quantity: > 0
        /// </remarks>
        [HttpPut]
        [Permission("import-good.update")]
        public async Task<ActionResult> Put([FromBody] UpdateImportGoodsCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Nhận viên thay đổi trạng thái danh sách sản phẩm nhập hàng
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - SupplierOrderId: Id của đơn nhập hàng
        /// - IsCancel: true - Huỷ đơn
        /// /// - IsCancel: false - Xác nhận nhập hàng vào kho
        /// </remarks>
        [HttpPatch]
        [Permission("import-good.change-status")]
        public async Task<ActionResult> ChangeStatus([FromBody] ChangeStatusImportGoodsCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
