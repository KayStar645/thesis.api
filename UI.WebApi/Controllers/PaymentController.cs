using Core.Application.Exceptions;
using Core.Application.Features.Payments.Commands.CreatePayment;
using Core.Application.Features.Payments.Commands.DeletePayment;
using Core.Application.Features.Payments.Commands.UpdatePayment;
using Core.Application.Features.Payments.Queries.DetailPayment;
using Core.Application.Features.Payments.Queries.ListPayment;
using Core.Application.Models;
using Core.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách phương thức thanh toán
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("payment.view")]
        public async Task<ActionResult> Get([FromQuery] ListPaymentCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin PT thanh toán theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("payment.view")]
        public async Task<ActionResult> Get([FromQuery] DetailPaymentCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thêm mới phương thức thanh toán
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - InternalCode: string, required, max(50)
        /// - Name: string, required, max(190)
        /// </remarks>
        [HttpPost]
        [Permission("payment.create")]
        public async Task<ActionResult> Post([FromBody] CreatePaymentCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Sửa thông tin phương thức thanh toán
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpPut]
        [Permission("payment.update")]
        public async Task<ActionResult> Put([FromBody] UpdatePaymentCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Xóa phương thức thanh toán
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpDelete]
        [Permission("payment.delete")]
        public async Task<ActionResult> Delete([FromQuery] DeletePaymentCommand pRequest)
        {
            try
            {
                await _mediator.Send(pRequest);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (NotFoundException ex)
            {
                var responses = Result<PaymentDto>.Failure(ex.Message, StatusCodes.Status404NotFound);
                return StatusCode(responses.Code, responses);
            }
            catch (BadRequestException ex)
            {
                var responses = Result<PaymentDto>.Failure(ex.Message, StatusCodes.Status400BadRequest);
                return StatusCode(responses.Code, responses);
            }
            catch (Exception ex)
            {
                var responses = Result<PaymentDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
                return StatusCode(responses.Code, responses);
            }
        }

    }
}
