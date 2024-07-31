using Core.Application.Exceptions;
using Core.Application.Features.Distributors.Commands.CreateDistributor;
using Core.Application.Features.Distributors.Commands.DeleteDistributor;
using Core.Application.Features.Distributors.Commands.UpdateDistributor;
using Core.Application.Features.Distributors.Queries.DetailDistributor;
using Core.Application.Features.Distributors.Queries.ListDistributor;
using Core.Application.Models;
using Core.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class DistributorController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DistributorController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách nhà cung cấp
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("distributor.view")]
        public async Task<ActionResult> Get([FromQuery] ListDistributorCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin NCC theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("distributor.view")]
        public async Task<ActionResult> Get([FromQuery] DetailDistributorCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thêm NCC mới
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - InternalCode: string, required, max(50)
        /// - Name: string, required, max(190)
        /// - Phone: string, lenght(10)
        /// - Email: string, email_format
        /// </remarks>
        [HttpPost]
        [Permission("distributor.create")]
        public async Task<ActionResult> Post([FromBody] CreateDistributorCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Sửa thông tin NCC
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpPut]
        [Permission("distributor.update")]
        public async Task<ActionResult> Put([FromBody] UpdateDistributorCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Xóa nhà cung cấp theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpDelete]
        [Permission("distributor.delete")]
        public async Task<ActionResult> Delete([FromQuery] DeleteDistributorCommand pRequest)
        {
            try
            {
                await _mediator.Send(pRequest);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (NotFoundException ex)
            {
                var responses = Result<DistributorDto>.Failure(ex.Message, StatusCodes.Status404NotFound);
                return StatusCode(responses.Code, responses);
            }
            catch (BadRequestException ex)
            {
                var responses = Result<DistributorDto>.Failure(ex.Message, StatusCodes.Status400BadRequest);
                return StatusCode(responses.Code, responses);
            }
            catch (Exception ex)
            {
                var responses = Result<DistributorDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
                return StatusCode(responses.Code, responses);
            }
        }

    }
}
