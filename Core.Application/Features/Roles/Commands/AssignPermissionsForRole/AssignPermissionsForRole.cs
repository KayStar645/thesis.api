using Core.Application.Common.Interfaces;
using Core.Application.Features.Roles.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Roles.Commands.AssignPermissionsForRole
{
    public record AssignPermissionsForRoleCommand : IRequest<Result<RoleDto>>
    {
        public int RoleId { get; set; }

        public List<string>? PermissionsName { get; set; }
    }

    public class AssignPermissionsForRoleHandler : IRequestHandler<AssignPermissionsForRoleCommand, Result<RoleDto>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMapper _mapper;
        protected readonly IMediator _mediator;

        public AssignPermissionsForRoleHandler(
            ISupermarketDbContext pContext,
            IMapper pMapper,
            IMediator mediator)
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = mediator;
        }

        public async Task<Result<RoleDto>> Handle(AssignPermissionsForRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new AssignPermissionsForRoleValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<RoleDto>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                var currentPermissions = await _context.RolePermissions
                                                .Where(x => x.RoleId == request.RoleId)
                                                .Include(x => x.Permission)
                                                .Select(x => x.Permission.Name)
                                                .ToListAsync();

                var addPermission = request.PermissionsName.Except(currentPermissions).ToList();
                var deletePermission = currentPermissions.Except(request.PermissionsName).ToList();

                await _mediator.Publish(new AfterAssignPermissionsForRoleEvent
                    (request.RoleId, addPermission, deletePermission));

                var role = await _context.Roles
                            .Include(x => x.RolePermissions)
                            .ThenInclude(x => x.Permission)
                            .Where(x => x.Id == request.RoleId)
                            .SingleOrDefaultAsync();

                var userDto = _mapper.Map<RoleDto>(role);

                return Result<RoleDto>.Success(userDto, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<RoleDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
