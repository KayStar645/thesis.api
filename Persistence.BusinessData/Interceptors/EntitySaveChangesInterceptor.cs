using Core.Application.Common.Interfaces;
using Core.Domain.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Persistence.BusinessData.Common;

namespace Persistence.BusinessData.Interceptors
{
    public class EntitySaveChangesInterceptor : SaveChangesInterceptor
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTime;

        public EntitySaveChangesInterceptor(ICurrentUserService currentUserService, IDateTimeService dateTime)
        {
            _currentUserService = currentUserService;
            _dateTime = dateTime;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context is null)
                return;
            

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditableEntity auditableEntity)
                {
                    auditableEntity.CreatedAt = _dateTime.Now;
                    auditableEntity.CreatedBy = _currentUserService.UserId.ToString();
                    auditableEntity.UpdatedAt = _dateTime.Now;
                    auditableEntity.UpdatedBy = _currentUserService.UserId.ToString();

                    if (entry.State == EntityState.Added)
                    {
                        auditableEntity.IsDeleted = false;
                    }
                    else if (entry.State == EntityState.Deleted && 
                        !CommonBusinessData.ImmediateDeleteTypes.Contains(entry.Entity.GetType()))
                    {
                        entry.State = EntityState.Unchanged;
                        auditableEntity.IsDeleted = true;
                    }
                }

                //if (entry.Entity is IHardDeleteEntity hardDeleteEntity && entry.State == EntityState.Added)
                //{
                //    hardDeleteEntity.CreatedAt = _dateTime.Now;
                //    hardDeleteEntity.CreatedBy = _currentUserService.UserId.ToString();
                //    hardDeleteEntity.UpdatedAt = _dateTime.Now;
                //    hardDeleteEntity.UpdatedBy = _currentUserService.UserId.ToString();
                //}
            }

        }
    }
}
