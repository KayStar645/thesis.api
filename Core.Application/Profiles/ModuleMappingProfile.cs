using Core.Application.Features.Categories.Commands.CreateCategory;
using Core.Application.Features.Categories.Commands.UpdatgeCategory;
using Core.Application.Features.Categories.Queries.ListCategory;
using Core.Application.Features.Coupons.Commands.CreateCoupon;
using Core.Application.Features.Coupons.Commands.UpdateCoupon;
using Core.Application.Features.Coupons.Queries.ListCoupon;
using Core.Application.Features.Customers.Commands.UpdateCustomer;
using Core.Application.Features.Customers.Queries.ListCustomer;
using Core.Application.Features.Deliveries.Commands.CreateDelivery;
using Core.Application.Features.Deliveries.Commands.UpdateDelivery;
using Core.Application.Features.Deliveries.Queries.ListDelivery;
using Core.Application.Features.Distributors.Commands.CreateDistributor;
using Core.Application.Features.Distributors.Commands.UpdateDistributor;
using Core.Application.Features.Distributors.Queries.ListDistributor;
using Core.Application.Features.ImportGoods.Queries.ListImportGood;
using Core.Application.Features.Orders.Queries.ListOrder;
using Core.Application.Features.Payments.Commands.CreatePayment;
using Core.Application.Features.Payments.Commands.UpdatePayment;
using Core.Application.Features.Payments.Queries.ListPayment;
using Core.Application.Features.Products.Commands.CreateProduct;
using Core.Application.Features.Products.Commands.UpdateProduct;
using Core.Application.Features.Products.Queries.ListProduct;
using Core.Application.Features.Products.Queries.ListPromotionComboProduct;
using Core.Application.Features.Promotions.Commands.CreatePromotion;
using Core.Application.Features.Promotions.Commands.UpdatePromotion;
using Core.Application.Features.Promotions.Queries.ListPromotion;
using Core.Application.Features.Roles.Commands.CreateRole;
using Core.Application.Features.Roles.Commands.UpdateRole;
using Core.Application.Features.Roles.Queries.ListRole;
using Core.Application.Features.Roles.Queries.ListRoleWithPermissionWithPermission;
using Core.Application.Features.StaffPositions.Commands.CreateStaffPosition;
using Core.Application.Features.StaffPositions.Commands.UpdateStaffPosition;
using Core.Application.Features.StaffPositions.Queries.ListStaffPosition;
using Core.Application.Features.Staffs.Commands.CreateStaff;
using Core.Application.Features.Staffs.Commands.UpdateStaff;
using Core.Application.Features.Staffs.Queries.ListStaff;
using Core.Application.Features.SupplierOrders.Commands.CreateSupplierOrder;
using Core.Application.Features.SupplierOrders.Queries.ListSupplierOrder;
using Core.Application.Features.Users.Commands.CreateUser;
using Core.Application.Features.Users.Commands.RegisterAccount;
using Core.Application.Features.Users.Commands.UpdateUser;
using Core.Application.Features.Users.Queries.GetListUser;
using Core.Application.Models;
using Core.Domain.Auth;
using Core.Domain.Entities;
using Sieve.Models;

namespace Core.Application.Profiles
{
    public class ModuleMappingProfile : Profile
    {
        public ModuleMappingProfile()
        {
            CreateMap<SieveModel, ListStaffCommand>().ReverseMap();
            CreateMap<Staff, StaffDto>().ReverseMap();
            CreateMap<Staff, CreateStaffCommand>().ReverseMap();
            CreateMap<Staff, UpdateStaffCommand>().ReverseMap();

            CreateMap<SieveModel, ListCustomerCommand>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Customer, UpdateCustomerCommand>().ReverseMap();

            CreateMap<SieveModel, GetListUserCommand>().ReverseMap();
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.UserRoles.Select(ur => ur.Role.Name)))
                .ReverseMap()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
            CreateMap<User, CreateUserCommand>().ReverseMap();
            CreateMap<User, UpdateUserCommand>().ReverseMap();
            CreateMap<User, RegisterAccountCommand>().ReverseMap();

            CreateMap<SieveModel, ListRoleCommand>().ReverseMap();
            CreateMap<Role, RoleDto>()
                .ForMember(dest => dest.Permissions, opt => opt.MapFrom(src => src.RolePermissions.Select(ur => ur.Permission.Name)))
                .ReverseMap()
                .ForMember(dest => dest.UserRoles, opt => opt.Ignore());
            CreateMap<SieveModel, ListRoleWithPermissionCommand>().ReverseMap();
            CreateMap<Role, CreateRoleCommand>().ReverseMap();
            CreateMap<Role, UpdateRoleCommand>().ReverseMap();

            CreateMap<SieveModel, ListDistributorCommand>().ReverseMap();
            CreateMap<Distributor, DistributorDto>().ReverseMap();
            CreateMap<Distributor, CreateDistributorCommand>().ReverseMap();
            CreateMap<Distributor, UpdateDistributorCommand>().ReverseMap();

            CreateMap<SieveModel, ListCategoryCommand>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryCommand>().ReverseMap();
            CreateMap<Category, UpdateCategoryCommand>().ReverseMap();

            CreateMap<SieveModel, ListProductCommand>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Product, CreateProductCommand>().ReverseMap();
            CreateMap<Product, UpdateProductCommand>().ReverseMap();

            CreateMap<SupplierOrder, SupplierOrderDto>().ReverseMap();
            CreateMap<SupplierOrder, CreateSupplierOrderCommand>().ReverseMap();

            CreateMap<SieveModel, ListPaymentCommand>().ReverseMap();
            CreateMap<Payment, PaymentDto>().ReverseMap();
            CreateMap<Payment, CreatePaymentCommand>().ReverseMap();
            CreateMap<Payment, UpdatePaymentCommand>().ReverseMap();

            CreateMap<SieveModel, ListStaffPositionCommand>().ReverseMap();
            CreateMap<StaffPosition, StaffPositionDto>().ReverseMap();
            CreateMap<StaffPosition, CreateStaffPositionCommand>().ReverseMap();
            CreateMap<StaffPosition, UpdateStaffPositionCommand>().ReverseMap();

            CreateMap<SieveModel, ListPromotionCommand>().ReverseMap();
            CreateMap<Promotion, PromotionDto>().ReverseMap();
            CreateMap<Promotion, CreatePromotionCommand>().ReverseMap();
            CreateMap<Promotion, UpdatePromotionCommand>().ReverseMap();

            CreateMap<SieveModel, ListCouponCommand>().ReverseMap();
            CreateMap<Coupon, CouponDto>().ReverseMap();
            CreateMap<Coupon, CreateCouponCommand>().ReverseMap();
            CreateMap<Coupon, UpdateCouponCommand>().ReverseMap();

            CreateMap<SieveModel, ListOrderCommand>().ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            CreateMap<Order, CartDto>().ReverseMap();
            CreateMap<DetailOrder, DetailOrderDto>().ReverseMap();
            CreateMap<DetailOrder, DetailCartDto>().ReverseMap();

            CreateMap<SieveModel, ListSupplierOrderCommand>().ReverseMap();
            CreateMap<SupplierOrder, SupplierOrderDto>().ReverseMap();
            CreateMap<DetailSupplierOrder, DetailSupplierOrderDto>().ReverseMap();

            CreateMap<SieveModel, ListImportGoodCommand>().ReverseMap();
            CreateMap<SupplierOrder, ImportGoodDto>().ReverseMap();

            CreateMap<SieveModel, ListDeliveryCommand>().ReverseMap();
            CreateMap<Delivery, DeliveryDto>().ReverseMap();
            CreateMap<Delivery, CreateDeliveryCommand>().ReverseMap();
            CreateMap<Delivery, UpdateDeliveryCommand>().ReverseMap();

            CreateMap<SieveModel, ListPromotionComboProductCommand>().ReverseMap();
        }
    }
}
