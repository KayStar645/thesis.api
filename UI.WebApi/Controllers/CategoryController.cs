using Core.Application.Exceptions;
using Core.Application.Features.Categories.Commands.CreateCategory;
using Core.Application.Features.Categories.Commands.DeleteCategory;
using Core.Application.Features.Categories.Commands.UpdatgeCategory;
using Core.Application.Features.Categories.Queries.DetailCategory;
using Core.Application.Features.Categories.Queries.ListCategory;
using Core.Application.Models;
using Core.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách nhóm sản phẩm
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("category.view")]
        public async Task<ActionResult> Get([FromQuery] ListCategoryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy nhóm sản phẩm theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("category.view")]
        public async Task<ActionResult> Get([FromQuery] DetailCategoryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thêm nhóm sản phẩm mới
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - InternalCode: string, required, max(50)
        /// - Name: string, required, max(190)
        /// </remarks>
        [HttpPost]
        [Permission("category.create")]
        public async Task<ActionResult> Post([FromBody] CreateCategoryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Sửa nhóm sản phẩm
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpPut]
        [Permission("category.update")]
        public async Task<ActionResult> Put([FromBody] UpdateCategoryCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Xóa nhóm sản phẩm
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpDelete]
        [Permission("category.delete")]
        public async Task<ActionResult> Delete([FromQuery] DeleteCategoryCommand pRequest)
        {
            try
            {
                await _mediator.Send(pRequest);
                return StatusCode(StatusCodes.Status204NoContent);
            }
            catch (NotFoundException ex)
            {
                var responses = Result<CategoryDto>.Failure(ex.Message, StatusCodes.Status404NotFound);
                return StatusCode(responses.Code, responses);
            }
            catch (BadRequestException ex)
            {
                var responses = Result<CategoryDto>.Failure(ex.Message, StatusCodes.Status400BadRequest);
                return StatusCode(responses.Code, responses);
            }
            catch (Exception ex)
            {
                var responses = Result<CategoryDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
                return StatusCode(responses.Code, responses);
            }
        }

    }
}
