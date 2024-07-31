using Core.Application.Common.Interfaces;
using Core.Domain.Auth;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Users.Events
{
    public class AfterAssignRolesForUserEvent : INotification
    {
        public int UserId { get; set; }

        public List<int>? AddRole { get; set; }

        public List<int>? DeleteRole { get; set; }

        public AfterAssignRolesForUserEvent(int pUserId, List<int> pAddRole, List<int>? pDeleteRole)
        {
            UserId = pUserId;
            AddRole = pAddRole;
            DeleteRole = pDeleteRole;
        }
    }

    public class AfterAssignRolesForUserCreateUserRoleEvent : 
        INotificationHandler<AfterAssignRolesForUserEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterAssignRolesForUserCreateUserRoleEvent> _logger;

        public AfterAssignRolesForUserCreateUserRoleEvent(
            ILogger<AfterAssignRolesForUserCreateUserRoleEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterAssignRolesForUserEvent notification, CancellationToken cancellationToken)
        {
            if(notification.DeleteRole != null)
            {
                foreach (var role in notification.DeleteRole)
                {
                    var result = await _context.UserRoles
                                .FirstOrDefaultAsync(x => x.UserId == notification.UserId && x.RoleId == role);
                    _context.UserRoles.Remove(result);
                }
            }

            if(notification.AddRole != null)
            {
                foreach (var role in notification.AddRole)
                {
                    await _context.UserRoles.AddAsync(new UserRole
                    {
                        UserId = notification.UserId,
                        RoleId = role
                    });
                }
            }
            await _context.SaveChangesAsync(default(CancellationToken));

            await Task.CompletedTask;
        }
    }
}
