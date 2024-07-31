using Core.Application.Common.Interfaces;
using Core.Application.Features.Users.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Users.Commands.AssignRolesForUser
{
    public record AssignRolesForUserCommand : IRequest<Result<UserDto>>
    {
        public int UserId { get; set; }

        public List<int>? RolesId { get; set; }
    }

    public class AssignRolesForUserCommandHandler : IRequestHandler<AssignRolesForUserCommand, Result<UserDto>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMapper _mapper;
        protected readonly IMediator _mediator;

        public AssignRolesForUserCommandHandler(
            ISupermarketDbContext pContext,
            IMapper pMapper,
            IMediator mediator)
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = mediator;
        }
        public async Task<Result<UserDto>> Handle(AssignRolesForUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new AssignRolesForUserValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<UserDto>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                var currentRoles = await _context.UserRoles
                                                .Where(x => x.UserId == request.UserId)
                                                .Select(x => x.RoleId)
                                                .ToListAsync();

                var addRole = request.RolesId.Except(currentRoles.Cast<int>()).ToList();
                var deleteRole = currentRoles.Cast<int>().Except(request.RolesId).ToList();

                await _mediator.Publish(new AfterAssignRolesForUserEvent(request.UserId , addRole, deleteRole));

                var user = await _context.Users
                            .Include(x => x.UserRoles)
                            .ThenInclude(x => x.Role)
                            .Include(x => x.Staff)
                            .Where(x => x.Id == request.UserId)
                            .SingleOrDefaultAsync();

                var userDto = _mapper.Map<UserDto>(user);

                return Result<UserDto>.Success(userDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<UserDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
