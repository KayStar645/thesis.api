using Core.Application.Common.Interfaces;
using Core.Application.Models;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Users.Commands.LinkUserToStaff
{
    public record LinkUserToStaffCommand : IRequest<Result<UserDto>>
    {
        public int UserId { get; set; }

        public int StaffId { get; set; }
    }

    public class LinkUserToStaffCommandHandler : IRequestHandler<LinkUserToStaffCommand, Result<UserDto>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMapper _mapper;

        public LinkUserToStaffCommandHandler(
            ISupermarketDbContext pContext,
            IMapper pMapper)
        {
            _context = pContext;
            _mapper = pMapper;
        }

        public async Task<Result<UserDto>> Handle(LinkUserToStaffCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new LinkUserToStaffValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<UserDto>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                var user = await _context.Users.Where(x => x.Id == request.UserId)
                                               .FirstOrDefaultAsync();
                user.StaffId = request.StaffId;
                _context.Users.Update(user);
                await _context.SaveChangesAsync(default(CancellationToken));

                var newUser = await _context.Users
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .Include(x => x.Staff)
                            .Where(x => x.Id == request.UserId)
                            .SingleOrDefaultAsync();

                var userDto = _mapper.Map<UserDto>(newUser);

                return Result<UserDto>.Success(userDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
