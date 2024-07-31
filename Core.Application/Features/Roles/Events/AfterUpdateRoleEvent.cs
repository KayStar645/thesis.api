using Core.Application.Common.Interfaces;
using Core.Application.Features.Roles.Commands.UpdateRole;
using Core.Domain.Auth;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Roles.Events
{
    public class AfterUpdateRoleEvent : INotification
    {
        public UpdateRoleCommand Request { get; set; }

        public AfterUpdateRoleEvent(UpdateRoleCommand pRequest)
        {
            Request = pRequest;
        }
    }

    public class AfterUpdateRoleUpdatePermissionEvent :
        INotificationHandler<AfterUpdateRoleEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterUpdateRoleUpdatePermissionEvent> _logger;

        public AfterUpdateRoleUpdatePermissionEvent(
            ILogger<AfterUpdateRoleUpdatePermissionEvent> pLogger,
            ISupermarketDbContext pContext)
        {                                                                                                                                                                                                                                                                                                                                                                                     
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterUpdateRoleEvent notification, CancellationToken cancellationToken)
        {
            var oldPermissions = await _context.RolePermissions
                       .Include(x => x.Permission)
                       .Where(x => x.Role.Name == notification.Request.Name)
                       .Select(x => x.Permission.Name)
                       .ToListAsync();

            var newPermissions = notification.Request.Permissions;

            var create = newPermissions.Except(oldPermissions).ToList();
            var delete = oldPermissions.Except(newPermissions).ToList();

            for(int i = 0; i < create.Count(); i++)
            {
                var permission = await _context.Permissions
                            .FirstOrDefaultAsync(x => x.Name == create[i]);

                var per = new RolePermission
                {
                    RoleId = (int)notification.Request.Id,
                    PermissionId = permission.Id
                };
                await _context.RolePermissions .AddAsync(per);
            }
            await _context.SaveChangesAsync(cancellationToken);

            for (int i = 0; i < delete.Count(); i++)
            {
                var del = await _context.RolePermissions
                            .Include(x => x.Permission)
                            .Where(x => x.RoleId == notification.Request.Id &&
                                        x.Permission.Name == delete[i])
                                        .FirstOrDefaultAsync();
                _context.RolePermissions.Remove(del);
            }
            await _context.SaveChangesAsync(cancellationToken);

            await Task.CompletedTask;
        }
    }
}
