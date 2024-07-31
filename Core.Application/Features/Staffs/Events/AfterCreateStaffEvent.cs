using Core.Application.Common.Interfaces;
using Core.Application.Features.Staffs.Commands.CreateStaff;
using Core.Domain.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static Core.Domain.Auth.User;

namespace Core.Application.Features.Staffs.Events
{
    public class AfterCreateStaffEvent : INotification
    {
        public CreateStaffCommand? Request { get; set; }

        public AfterCreateStaffEvent(CreateStaffCommand? request)
        {
            Request = request;
        }
    }

    public class AfterCreateStaffCreateUserAndAddRoleEvent :
        INotificationHandler<AfterCreateStaffEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterCreateStaffCreateUserAndAddRoleEvent> _logger;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AfterCreateStaffCreateUserAndAddRoleEvent(
            ILogger<AfterCreateStaffCreateUserAndAddRoleEvent> pLogger,
            ISupermarketDbContext pContext, IPasswordHasher<User> pPasswordHasher)
        {
            _logger = pLogger;
            _context = pContext;
            _passwordHasher = pPasswordHasher;
        }

        public async Task Handle(AfterCreateStaffEvent notification, CancellationToken cancellationToken)
        {
            // Tạo tài khoản
            var staff = await _context.Staffs
                .Where(x => x.InternalCode == notification.Request.InternalCode)
                .FirstOrDefaultAsync();

            var user = new User
            {
                UserName = notification.Request.InternalCode,
                Email = notification.Request.Email,
                PhoneNumber = notification.Request.PhoneNumber,
                Type = UserType.SuperAdmin,
                StaffId = staff.Id,
            };
            user.Password = _passwordHasher.HashPassword(user, notification.Request.InternalCode);

            var entity = await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync(cancellationToken);

            await Task.CompletedTask;
        }
    }
}
