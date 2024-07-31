using Core.Application.Exceptions;
using Core.Application.Features.StaffPositions.Commands.CreateStaffPosition;
using Core.Application.Features.StaffPositions.Commands.DeleteStaffPosition;
using Core.Application.Features.StaffPositions.Commands.UpdateStaffPosition;
using Core.Application.Features.StaffPositions.Queries.DetailStaffPosition;
using Core.Application.Features.StaffPositions.Queries.ListStaffPosition;
using Core.Application.Models;
using Core.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class StaffPositionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StaffPositionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách vị trí nhân viên
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("staff-position.view")]
        public async Task<ActionResult> Get([FromQuery] ListStaffPositionCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin vị trí nhân viên theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("staff-position.view")]
        public async Task<ActionResult> Get([FromQuery] DetailStaffPositionCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thêm mới vị trí nhân viên
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - InternalCode: string, required, max(50)
        /// - Name: string, required, max(190)
        /// - Describes: ckeditor
        /// </remarks>
        [HttpPost]
        [Permission("staff-position.create")]
        public async Task<ActionResult> Post([FromBody] CreateStaffPositionCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Sửa thông tin vị trí nhân viên
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpPut]
        [Permission("staff-position.update")]
        public async Task<ActionResult> Put([FromBody] UpdateStaffPositionCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Xóa vị trí nhân viên
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpDelete]
        [Permission("staff-position.delete")]
        public async Task<ActionResult> Delete([FromQuery] DeleteStaffPositionCommand pRequest)
        {
            try
            {
                await _mediator.Send(pRequest);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (NotFoundException ex)
            {
                var responses = Result<StaffPositionDto>.Failure(ex.Message, StatusCodes.Status404NotFound);
                return StatusCode(responses.Code, responses);
            }
            catch (BadRequestException ex)
            {
                var responses = Result<StaffPositionDto>.Failure(ex.Message, StatusCodes.Status400BadRequest);
                return StatusCode(responses.Code, responses);
            }
            catch (Exception ex)
            {
                var responses = Result<StaffPositionDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
                return StatusCode(responses.Code, responses);
            }
        }

    }
}
