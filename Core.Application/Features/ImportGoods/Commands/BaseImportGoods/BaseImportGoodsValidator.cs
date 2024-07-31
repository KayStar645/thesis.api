using Core.Application.Common.Interfaces;
using Core.Application.Transforms;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.ImportGoods.Commands.BaseImportGoods
{
    public class BaseImportGoodsValidator : AbstractValidator<IBaseImportGoods>
    {
        public BaseImportGoodsValidator(ISupermarketDbContext pContext, int? pSupplierOrderId)
        {
            RuleFor(x => x.ReceivingStaff)
                .NotEmpty()
                .WithMessage("Nhân viên hàng hàng không được phép bỏ trống!");

            RuleFor(x => x.Details)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.SupplierOrder.DetailSupplierOrder))
                .MustAsync(async (details, token) =>
                {
                    if (details != null)
                    {
                        foreach (var detail in details)
                        {
                            if (details.Count(x => x.ProductId == detail.ProductId) > 1)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }).WithMessage("Id của sản phẩm trong details bị trùng lặp!");

            RuleFor(x => x.Details)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.SupplierOrder.DetailSupplierOrder))
                .MustAsync(async (details, token) =>
                {
                    if (details != null)
                    {
                        foreach (var detail in details)
                        {
                            if (detail.ProductId == null || detail.ImportQuantity <= 0)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }).WithMessage("Giá và số lượng nhập phải lớn hơn 0!");

            RuleFor(x => x.Details)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.SupplierOrder.DetailSupplierOrder))
                .MustAsync(async (details, token) =>
                {
                    if (details != null)
                    {
                        foreach (var detail in details)
                        {
                            var countProductOrder = await pContext.DetailSupplierOrders
                                .Include(x => x.SupplierOrder)
                                .Include(x => x.Product)
                                .Where(x => x.SupplierOrder.Id == pSupplierOrderId &&
                                     x.ProductId == detail.ProductId &&
                                     (x.SupplierOrder.Status == SupplierOrderStatus.Order ||
                                     x.SupplierOrder.Status == SupplierOrderStatus.PartialReceipt))
                                .SumAsync(x => x.Quantity);

                            var countProductImport = await pContext.DetailSupplierOrders
                                .Include(x => x.Product)
                                .Include(x => x.SupplierOrder)
                                .Where(x => x.SupplierOrder.ParentId == pSupplierOrderId &&
                                     x.ProductId == detail.ProductId)
                                .SumAsync(x => x.Quantity);

                            if (countProductImport + detail.ImportQuantity > countProductOrder)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }).WithMessage("Số lượng nhập không được lớn hơn số lượng đặt!");

        }
    }
}
