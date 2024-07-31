using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Users.Commands.RegisterAccount;
using Core.Domain.Auth;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.Users.Events
{
    public class AfterRegisterAccountEvent : INotification
    {
        public RegisterAccountCommand? Request { get; set; }

        public AfterRegisterAccountEvent(RegisterAccountCommand pRequest)
        {
            Request = pRequest;
        }
    }

    public class AfterRegisterAccountCreateCustomerEvent :
        INotificationHandler<AfterRegisterAccountEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterRegisterAccountCreateCustomerEvent> _logger;

        public AfterRegisterAccountCreateCustomerEvent(
            ILogger<AfterRegisterAccountCreateCustomerEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterRegisterAccountEvent notification, CancellationToken cancellationToken)
        {
            var customer = new Customer
            {
                Name = notification.Request.Name,
                Email = notification.Request.Email,
                Phone = notification.Request.PhoneNumber,
                Address = notification.Request.Address,
                Gender = notification.Request.Gender,
            };
            var newCustomer = await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync(default(CancellationToken));

            var user = await _context.Users
                .Where(x => x.UserName == notification.Request.UserName)
                .FirstOrDefaultAsync();
            user.CustomerId = newCustomer.Entity.Id;
            _context.Users.Update(user);
            await _context.SaveChangesAsync(default(CancellationToken));

            await Task.CompletedTask;
        }
    }

    public class AfterRegisterAccountAssignRolesForUserEvent :
        INotificationHandler<AfterRegisterAccountEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterRegisterAccountCreateCustomerEvent> _logger;

        public AfterRegisterAccountAssignRolesForUserEvent(
            ILogger<AfterRegisterAccountCreateCustomerEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterRegisterAccountEvent notification, CancellationToken cancellationToken)
        {
            foreach (string permission in CONSTANT_PERMESSION_DEFAULT.PERMISSIONS)
            {
                var p = await _context.Permissions
                    .Where(x => x.Name == permission)
                    .SingleOrDefaultAsync();
                var user = await _context.Users
                    .Where(x => x.UserName == notification.Request.UserName)
                    .SingleOrDefaultAsync();
                if(p != null)
                {
                    var userPermission = new UserPermission
                    {
                        PermissionId = p.Id,
                        UserId = user.Id
                    };
                    await _context.UserPermissions.AddAsync(userPermission);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }

            await Task.CompletedTask;
        }
    }
}
