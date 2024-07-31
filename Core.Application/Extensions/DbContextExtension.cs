using Core.Domain.Common;

namespace Core.Application.Extensions
{
    public static class DbContextExtension
    {
        public static IQueryable<T> FilterDeleted<T>(this DbSet<T> dbSet) where T : AuditableEntity
        {
            return dbSet.Where(e => (bool)!e.IsDeleted);
        }
    }
}
