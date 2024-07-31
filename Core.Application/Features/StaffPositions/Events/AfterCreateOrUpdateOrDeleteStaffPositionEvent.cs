
using Core.Application.Common.Interfaces;
using Core.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Core.Application.Features.StaffPositions.Events
{
    public class AfterCreateOrUpdateOrDeleteStaffPositionEvent : INotification
    {
        public int? Id { get; set; }

        public string? InternalCode { get; set; }

        public List<int?>? Roles { get; set; }

        public AfterCreateOrUpdateOrDeleteStaffPositionEvent(int? pId, string? internalCode, List<int?>? roles)
        {
            Id = pId;
            InternalCode = internalCode;
            Roles = roles;
        }
    }

    public class AfterCreateOrUpdateOrDeleteStaffPositionUpdateStaffPrositionRoleEvent :
        INotificationHandler<AfterCreateOrUpdateOrDeleteStaffPositionEvent>
    {
        private readonly ISupermarketDbContext _context;
        private readonly ILogger<AfterCreateOrUpdateOrDeleteStaffPositionUpdateStaffPrositionRoleEvent> _logger;

        public AfterCreateOrUpdateOrDeleteStaffPositionUpdateStaffPrositionRoleEvent(
            ILogger<AfterCreateOrUpdateOrDeleteStaffPositionUpdateStaffPrositionRoleEvent> pLogger,
            ISupermarketDbContext pContext)
        {
            _logger = pLogger;
            _context = pContext;
        }

        public async Task Handle(AfterCreateOrUpdateOrDeleteStaffPositionEvent notification, CancellationToken cancellationToken)
        {
            if (notification.InternalCode != null && notification.Id == null)
            {
                var staffPosition = await _context.StaffPositions
                    .Where(x => x.InternalCode == notification.InternalCode)
                    .FirstOrDefaultAsync();
                foreach(var role in notification.Roles)
                {
                    var spRole = new StaffPositionHasRole
                    {
                        RoleId = role,
                        StaffPositionId = staffPosition.Id
                    };
                    await _context.StaffPositionHasRoles.AddAsync(spRole);
                    await _context.SaveChangesAsync(cancellationToken);
                }    
            }
            else if (notification.InternalCode != null && notification.Id != null)
            {
                var oldRoles = await _context.StaffPositionHasRoles
                    .Where(x => x.StaffPositionId == notification.Id)
                    .Select(x => x.RoleId)
                    .ToListAsync();
                var creates = notification.Roles.Except(oldRoles).ToList();
                var deletes = oldRoles.Except(notification.Roles).ToList();

                foreach(var role in creates)
                {
                    var spRole = new StaffPositionHasRole
                    {
                        RoleId = role,
                        StaffPositionId = notification.Id
                    };
                    await _context.StaffPositionHasRoles.AddAsync(spRole);
                    await _context.SaveChangesAsync(cancellationToken);
                }

                foreach(var role in deletes)
                {
                    var spRole = await _context.StaffPositionHasRoles
                        .Where(x => x.StaffPositionId == notification.Id &&
                                    x.RoleId == role)
                        .SingleOrDefaultAsync();
                    _context.StaffPositionHasRoles.Remove(spRole);
                    await _context.SaveChangesAsync(cancellationToken);
                }    
            }    
            else
            {
                var spRoles = await _context.StaffPositionHasRoles
                    .Where(x => x.StaffPositionId == notification.Id)
                    .ToListAsync();

                _context.StaffPositionHasRoles.RemoveRange(spRoles);
                await _context.SaveChangesAsync(cancellationToken);
            }

            await Task.CompletedTask;
        }
    }
}
