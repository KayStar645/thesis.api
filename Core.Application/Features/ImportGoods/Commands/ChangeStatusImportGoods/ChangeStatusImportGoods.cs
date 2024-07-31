using Core.Application.Common.Interfaces;
using Core.Application.Features.ImportGoods.Events;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.ImportGoods.Commands.ChangeStatusImportGoods
{
    public record ChangeStatusImportGoodsCommand : IRequest<Result<bool>>
    {
        public int? SupplierOrderId { get; set; }

        public bool? IsCancel { get; set; }
    }

    public class ChangeStatusImportGoodsCommandHandler :
        IRequestHandler<ChangeStatusImportGoodsCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public ChangeStatusImportGoodsCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(ChangeStatusImportGoodsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ChangeStatusImportGoodsValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                var order = await _context.SupplierOrders.FindAsync(request.SupplierOrderId);

                if (request.IsCancel == true)
                {
                    order.Status = SupplierOrderStatus.Cancel;
                }
                else
                {
                    order.Status = SupplierOrderStatus.Completed;

                    var child = await _context.SupplierOrders
                        .FindAsync(request.SupplierOrderId);
                    var supplierOrder = await _context.SupplierOrders
                        .FindAsync(child.ParentId);

                    // Nếu đã nhập hết thì sửa parent thành Completed; PartialReceipt
                    var details = await _context.DetailSupplierOrders
                        .Where(x => x.SupplierOrderId == supplierOrder.Id)
                        .ToListAsync();
                    bool flag = false;
                    foreach (var item in details)
                    {
                        // Kiểm tra nhập hết chưa
                        var countProduct = await _context.DetailSupplierOrders
                            .Include(x => x.SupplierOrder)
                            .Where(x => x.SupplierOrder.ParentId == supplierOrder.Id &&
                                        x.ProductId == item.ProductId)
                            .SumAsync(x => x.Quantity);
                        if(countProduct < item.Quantity)
                        {
                            supplierOrder.Status = SupplierOrderStatus.PartialReceipt;
                            flag = true;
                            break;
                        }
                    }
                    if(!flag)
                    {
                        supplierOrder.Status = SupplierOrderStatus.Completed;
                    }
                    _context.SupplierOrders.Update(supplierOrder);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                order.ApproveStaffId = _currentUserService.StaffId;

                _context.SupplierOrders.Update(order);
                await _context.SaveChangesAsync(cancellationToken);

                // Xác nhận nhập hàng: Thêm sự kiện cập nhật lại sản phẩm
                await _mediator.Publish(new AfterChangeStatusImportGoodEvent(request));

                return Result<bool>.Success(true, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
