using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Application.Features.Users.Commands.LoginAccount
{
    public record LoginAccountCommand : IRequest<Result<LoginDto>>
    {
        public string? UserName { get; set; }

        public string? Password { get; set; }
    }

    public class LoginAccountCommandHandler : IRequestHandler<LoginAccountCommand, Result<LoginDto>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;

        public LoginAccountCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IPasswordHasher<User> passwordHasher, IConfiguration pConfiguration)
        {
            _context = pContext;
            _mapper = pMapper;
            _passwordHasher = passwordHasher;
            _configuration = pConfiguration;
        }

        public async Task<Result<LoginDto>> Handle(LoginAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new LoginAccountValidator();
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<LoginDto>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                User? user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.UserName ||
                                        x.UserName == request.UserName || x.PhoneNumber == request.UserName);


                if (user == null)
                {
                    return Result<LoginDto>.Failure("Tài khoản không tồn tại!", StatusCodes.Status400BadRequest);
                }
                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

                if (result != PasswordVerificationResult.Success)
                {
                    return Result<LoginDto>.Failure("Thông tin xác thực không hợp lệ!", StatusCodes.Status400BadRequest);
                }

                JwtSecurityToken jwtSecurityToken = await GenerateToken(user);

                LoginDto auth = new LoginDto
                {
                    Id = user.Id,
                    Exp = DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:DurationInMinutes"])),
                    Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                };

                return Result<LoginDto>.Success(auth, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<LoginDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<JwtSecurityToken> GenerateToken(User pUser)
        {
            var roles = await _context.Roles
                    .Where(x => x.UserRoles.Any(x => x.UserId == pUser.Id))
                    .ToListAsync();
            var permissions = await _context.Permissions
                    .Where(x => x.RolePermissions.Any(x => x.Role.UserRoles.Any(x => x.UserId == pUser.Id)) ||
                                x.UserPermissions.Any(x => x.UserId == pUser.Id))
                    .ToListAsync();

            var positionId = await _context.Users
                .Where(x => x.Id == pUser.Id)
                .Select(x => x.Staff.PositionId)
                .SingleOrDefaultAsync();
            if(positionId != null)
            {
                var per = await _context.StaffPositionHasRoles
                    .Where(x => x.StaffPositionId == positionId)
                    .SelectMany(x => x.Role.RolePermissions.Select(p => p.Permission))
                    .ToListAsync();
                permissions = permissions.Union(per).Distinct().ToList();
            }

            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role.Name));
            var permissionClaims = permissions.Select(permission => new Claim(CONSTANT_CLAIM_TYPES.Permission, permission.Name));

            var claims = new[]
            {
                new Claim(CONSTANT_CLAIM_TYPES.Uid, pUser.Id.ToString()),
                new Claim(CONSTANT_CLAIM_TYPES.Type, pUser.Type.ToString()),
                new Claim(CONSTANT_CLAIM_TYPES.Staff, pUser.StaffId.ToString()),
                new Claim(CONSTANT_CLAIM_TYPES.Customer, pUser.CustomerId.ToString()),
                new Claim(CONSTANT_CLAIM_TYPES.UserName, pUser.UserName),
            }
            .Union(permissionClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
