using Core.Application.Features.Users.Commands.AssignRolesForUser;
using Core.Application.Features.Users.Commands.CreateUser;
using Core.Application.Features.Users.Commands.LinkUserToStaff;
using Core.Application.Features.Users.Commands.LoginAccount;
using Core.Application.Features.Users.Commands.RegisterAccount;
using Core.Application.Features.Users.Commands.UpdateUser;
using Core.Application.Features.Users.Queries.GetListUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.WebApi.Middleware;

namespace UI.WebApi.Controllers
{
    [Route("~/smw-api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy danh người dùng
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        ///
        /// </remarks>
        [HttpGet]
        [Permission("user.view")]
        public async Task<ActionResult> Get([FromQuery] GetListUserCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Tạo người dùng mới
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - UserName: string 3->50
        /// - Password: string 6->50
        /// - PhoneNumber: required
        /// - Email: required
        /// </remarks>
        [HttpPost]
        [Permission("user.create")]
        public async Task<ActionResult> Post([FromBody] CreateUserCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Đăng ký tài khoản người dùng mới
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - UserName: string 3->50
        /// - Password: string 6->50
        /// - PhoneNumber: required
        /// - Email: required
        /// </remarks>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody] RegisterAccountCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Đăng nhập người dùng
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - UserName: string 3->50
        /// - Password: string 6->50
        /// </remarks>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginAccountCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Gán vai trò cho người dùng
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - UserId: int
        /// - RolesId: array int
        /// </remarks>
        [HttpPost("assign")]
        [Permission("user.assign")]
        public async Task<ActionResult> AssignRolesForUser([FromBody] AssignRolesForUserCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Link người dùng tới tài khoản user supper admin
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - UserId: int
        /// - RolesId: array int
        /// </remarks>
        [HttpPatch("link-staff")]
        [Permission("user-staff.link")]
        public async Task<ActionResult> LinkUserToStaff([FromBody] LinkUserToStaffCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }

        /// <summary>
        /// Sửa thông tin người dùng
        /// </summary>
        /// <remarks>
        /// Ràng buộc: 
        /// - PhoneNumber: required
        /// - Email: required
        /// </remarks>
        [HttpPut]
        [Permission("user.update")]
        public async Task<ActionResult> Put([FromBody] UpdateUserCommand pRequest)
        {
            var response = await _mediator.Send(pRequest);

            return StatusCode(response.Code, response);
        }
    }
}
