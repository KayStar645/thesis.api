using Core.Application.Features.Products.Commands.ChangeStatusProduct;
using Core.Application.Features.Products.Commands.CreateProduct;
using Core.Application.Features.Products.Commands.UpdateProduct;
using Core.Application.Features.Products.Queries.DetailProduct;
using Core.Application.Features.Products.Queries.ListProduct;
using Core.Application.Features.Products.Queries.ListPromotionComboProduct;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh sách sản phẩm
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("product.view")]
        public async Task<ActionResult> Get([FromQuery] ListProductCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy danh sách sản phẩm theo nhóm khuyến mãi
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet("combo-products")]
        [Permission("product.view")]
        public async Task<ActionResult> Get([FromQuery] ListPromotionComboProductCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin sản phẩm theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("product.view")]
        public async Task<ActionResult> Get([FromQuery] DetailProductCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thêm sản phẩm mới
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - InternalCode: string, required, max(50)
        /// - Name: string, required, max(190)
        /// - Images: array url
        /// - Price: > 0
        /// - Describes: ckeditor
        /// - Feature: ckeditor
        /// - Specifications: ckeditor
        /// - CategoryId: id có trong Category
        /// </remarks>
        [HttpPost]
        [Permission("product.create")]
        public async Task<ActionResult> Post([FromBody] CreateProductCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Sửa thông tin sản phẩm
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpPut]
        [Permission("product.update")]
        public async Task<ActionResult> Put([FromBody] UpdateProductCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Nhận viên thay đổi trạng thái sản phẩm
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - OrderId: Id của sản phẩm
        /// - Status: Trạng thái thay đổi của đơn hàng
        /// Draft(0), Active(1), Pause(2), OutStock(3), Stop(4)
        /// </remarks>
        [HttpPatch]
        [Permission("product.change-status")]
        public async Task<ActionResult> Change([FromBody] ChangeStatusProductCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        ///// <summary>
        ///// Xóa sản phẩm 
        ///// </summary>
        ///// <remarks>
        ///// Ràng buộc: 
        ///// - Id: int, required
        ///// </remarks>
        //[HttpDelete]
        //[Permission("product.delete")]
        //public async Task<ActionResult> Delete([FromQuery] DeleteProductCommand pRequest)
        //{
        //    try
        //    {
        //        await _mediator.Send(pRequest);
        //        return StatusCode(StatusCodes.Status204NoContent);
        //    }
        //    catch (NotFoundException ex)
        //    {
        //        var responses = Result<ProductDto>.Failure(ex.Message, StatusCodes.Status404NotFound);
        //        return StatusCode(responses.Code, responses);
        //    }
        //    catch (BadRequestException ex)
        //    {
        //        var responses = Result<ProductDto>.Failure(ex.Message, StatusCodes.Status400BadRequest);
        //        return StatusCode(responses.Code, responses);
        //    }
        //    catch (Exception ex)
        //    {
        //        var responses = Result<ProductDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
        //        return StatusCode(responses.Code, responses);
        //    }
        //}
    }
}
