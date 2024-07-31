using Core.Application.Features.Roles.Commands.AssignPermissionsForRole;
using Core.Application.Features.Roles.Commands.CreateRole;
using Core.Application.Features.Roles.Commands.UpdateRole;
using Core.Application.Features.Roles.Queries.DetailRole;
using Core.Application.Features.Roles.Queries.ListRole;
using Core.Application.Features.Roles.Queries.ListRoleWithPermissionWithPermission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]

    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh vai trò
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("role.view")]
        public async Task<ActionResult> Get([FromQuery] ListRoleCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy danh vai trò theo controller
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet("list-with-controller")]
        [Permission("role.view")]
        public async Task<ActionResult> GetByController([FromQuery] ListRoleWithPermissionCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Lấy thông tin vai trò theo id
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// </remarks>
        [HttpGet("detail")]
        [Permission("role.view")]
        public async Task<ActionResult> Get([FromQuery] DetailRoleCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Thêm vai trò mới
        /// </summary>
        /// <remarks>
        /// Ràng buộc:
        /// - Name: string, required, max(190)
        /// </remarks>
        [HttpPost]
        [Permission("role.create")]
        public async Task<ActionResult> Post([FromBody] CreateRoleCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Sửa tên vai trò
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - Id: int, required
        /// - Name: string, required, max(190)
        /// </remarks>
        [HttpPut]
        [Permission("role.update")]
        public async Task<ActionResult> Put([FromBody] UpdateRoleCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Gán quyền cho vai trò
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - RoleId: int
        /// - PermessionsName: array string
        /// </remarks>
        [HttpPost("assign")]
        [Permission("role.assign")]
        public async Task<ActionResult> AssignPermissionForRole([FromBody] AssignPermissionsForRoleCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
