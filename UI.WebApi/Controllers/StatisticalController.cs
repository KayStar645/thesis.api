using Core.Application.Features.Statistical.Queries.StatisticalInventory;
using Core.Application.Features.Statistical.Queries.StatisticalOrder;
using Core.Application.Features.Statistical.Queries.StatisticalOverview;
using Core.Application.Features.Statistical.Queries.StatisticalRevenue;
using Core.Application.Features.Statistical.Queries.StatisticalSelling;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class StatisticalController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StatisticalController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy số liệu thống kê chung
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet("overview")]
        [Permission("statistical.view")]
        public async Task<ActionResult> Get([FromQuery] StatisticalOverviewCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Biểu đồ số lượng đơn hàng theo ngày tháng năm
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - IsYear: true - theo năm, false - theo tháng
        /// - Year: bắt buộc
        /// - Month: nếu IsYear = false thì bắt buộc
        ///
        /// </remarks>
        [HttpGet("order")]
        [Permission("statistical.view")]
        public async Task<ActionResult> Get([FromQuery] StatisticalOrderCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        ///  Biểu đồ thống kê số lượng tồn kho theo nhóm sản phẩm và theo từng sản phẩm
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet("inventory")]
        [Permission("statistical.view")]
        public async Task<ActionResult> Get([FromQuery] StatisticalInventoryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        ///  Biểu đồ nhóm sản phẩm bán chạy theo ngày tháng năm
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - IsYear: true - theo năm, false - theo tháng
        /// - Year: bắt buộc
        /// - Month: nếu IsYear = false thì bắt buộc
        ///
        /// </remarks>
        [HttpGet("selling")]
        [Permission("statistical.view")]
        public async Task<ActionResult> Get([FromQuery] StatisticalSellingCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        ///  Biểu đồ thống kê doanh thu theo ngày tháng năm
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - IsYear: true - theo năm, false - theo tháng
        /// - Year: bắt buộc
        /// - Month: nếu IsYear = false thì bắt buộc
        ///
        /// </remarks>
        [HttpGet("revenue")]
        [Permission("statistical.view")]
        public async Task<ActionResult> Get([FromQuery] StatisticalRevenueCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
