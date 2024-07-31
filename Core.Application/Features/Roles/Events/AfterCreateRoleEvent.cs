using Core.Application.Common.Interfaces;
using Core.Application.Features.Roles.Commands.CreateRole;
using Core.Domain.Auth;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Roles.Events
{
    public class AfterCreateRoleEvent : INotification
    {
        public CreateRoleCommand Request { get; set; }

        public AfterCreateRoleEvent(CreateRoleCommand pRequest)
        {
            Request = pRequest;
        }
    }

    public class AfterCreateRoleCreatePermissionEvent :
        INotificationHandler<AfterCreateRoleEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterCreateRoleCreatePermissionEvent> _logger;

        public AfterCreateRoleCreatePermissionEvent(
            ILogger<AfterCreateRoleCreatePermissionEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterCreateRoleEvent notification, CancellationToken cancellationToken)
        {

            var create = notification.Request.Permissions;
            var role = await _context.Roles
                                .Where(x => x.Name == notification.Request.Name)
                                .FirstOrDefaultAsync();

            for (int i = 0; i < create.Count(); i++)
            {
                var permission = await _context.Permissions
                            .FirstOrDefaultAsync(x => x.Name == create[i]);

                var per = new RolePermission
                {
                    RoleId = role.Id,
                    PermissionId = permission.Id
                };
                await _context.RolePermissions.AddAsync(per);
            }
            await _context.SaveChangesAsync(cancellationToken);

            await Task.CompletedTask;
        }
    }
}
