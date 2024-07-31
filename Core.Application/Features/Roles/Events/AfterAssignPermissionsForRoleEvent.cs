using Core.Application.Common.Interfaces;
using Core.Application.Features.Users.Events;
using Core.Domain.Auth;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Roles.Events
{
    public class AfterAssignPermissionsForRoleEvent : INotification
    {
        public int RoleId { get; set; }

        public List<string>? AddPermission { get; set; }

        public List<string>? DeletePermission { get; set; }

        public AfterAssignPermissionsForRoleEvent(int pRoleId, List<string> pAddPermission,
            List<string>? pDeletePermission)
        {
            RoleId = pRoleId;
            AddPermission = pAddPermission;
            DeletePermission = pDeletePermission;
        }
    }

    public class AfterAssignPermissionsForRoleCreateRolePermissionEvent :
        INotificationHandler<AfterAssignPermissionsForRoleEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterAssignPermissionsForRoleCreateRolePermissionEvent> _logger;

        public AfterAssignPermissionsForRoleCreateRolePermissionEvent(
            ILogger<AfterAssignPermissionsForRoleCreateRolePermissionEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterAssignPermissionsForRoleEvent notification, CancellationToken cancellationToken)
        {
            if (notification.DeletePermission != null)
            {
                foreach (var permission in notification.DeletePermission)
                {
                    var result = await _context.RolePermissions
                                .Include(x => x.Permission)
                                .FirstOrDefaultAsync(x => x.RoleId == notification.RoleId &&
                                x.Permission.Name == permission);
                    if(result != null)
                    {
                        _context.RolePermissions.Remove(result);
                    }
                }
            }

            if (notification.AddPermission != null)
            {
                foreach (var permission in notification.AddPermission)
                {
                    var per = await _context.Permissions.FirstOrDefaultAsync(x => x.Name == permission);
                    if(per != null)
                    {
                        await _context.RolePermissions.AddAsync(new RolePermission
                        {
                            RoleId = notification.RoleId,
                            PermissionId = per.Id
                        });
                    }  
                }
            }
            await _context.SaveChangesAsync(default(CancellationToken));

            await Task.CompletedTask;
        }
    }
}
