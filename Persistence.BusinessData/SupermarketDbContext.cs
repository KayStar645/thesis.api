using Core.Application.Common.Interfaces;
using Core.Domain.Auth;
using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.BusinessData.Interceptors;
using System.Reflection;

namespace Persistence.BusinessData
{
    public class SupermarketDbContext : DbContext, ISupermarketDbContext
    {
        private readonly EntitySaveChangesInterceptor _saveChangesInterceptor;

        public SupermarketDbContext(
            DbContextOptions options,
            EntitySaveChangesInterceptor saveChangesInterceptor)
            : base(options)
        {
            _saveChangesInterceptor = saveChangesInterceptor;
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<Role> Roles => Set<Role>();

        public DbSet<Permission> Permissions => Set<Permission>();

        public DbSet<UserRole> UserRoles => Set<UserRole>();

        public DbSet<RolePermission> RolePermissions => Set<RolePermission>();

        public DbSet<UserPermission> UserPermissions => Set<UserPermission>();

        public DbSet<Staff> Staffs => Set<Staff>();

        public DbSet<Customer> Customers => Set<Customer>();

        public DbSet<Distributor> Distributors => Set<Distributor>();

        public DbSet<Category> Categories => Set<Category>();

        public DbSet<Product> Products => Set<Product>();

        public DbSet<SupplierOrder> SupplierOrders => Set<SupplierOrder>();

        public DbSet<DetailSupplierOrder> DetailSupplierOrders => Set<DetailSupplierOrder>();

        public DbSet<Payment> Payments => Set<Payment>();

        public DbSet<StaffPosition> StaffPositions => Set<StaffPosition>();

        public DbSet<Promotion> Promotions => Set<Promotion>();

        public DbSet<PromotionProductRequirement> PromotionProductRequirements => Set<PromotionProductRequirement>();

        public DbSet<Coupon> Coupons => Set<Coupon>();

        public DbSet<Order> Orders => Set<Order>();

        public DbSet<Delivery> Deliveries => Set<Delivery>();

        public DbSet<DetailOrder> DetailOrders => Set<DetailOrder>();

        public DbSet<StaffPositionHasRole> StaffPositionHasRoles => Set<StaffPositionHasRole>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Order of the interceptors is important
            optionsBuilder.AddInterceptors(_saveChangesInterceptor);
        }

    }
}
