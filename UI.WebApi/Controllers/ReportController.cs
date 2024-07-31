using Core.Application.Features.Report.Queries.ReportProfit;
using Core.Application.Features.Report.Queries.ReportPromotionGroupProfit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReportController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy thông tin xuất báo cáo lợi nhuận
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet("profit")]
        [Permission("report.view")]
        public async Task<ActionResult> Get([FromQuery] ReportProfitCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin xuất báo cáo LỢI NHUẬN THEO NHÓM KHUYẾN MÃI
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet("promotion-group-profit")]
        [Permission("promotion-group-profit.view")]
        public async Task<ActionResult> Get([FromQuery] ReportPromotionGroupProfitCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
