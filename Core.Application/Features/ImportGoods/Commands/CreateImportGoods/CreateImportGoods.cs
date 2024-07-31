using Core.Application.Common.Interfaces;
using Core.Application.Extensions;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.ImportGoods.Commands.BaseImportGoods;
using Core.Application.Features.ImportGoods.Commands.ChangeStatusImportGoods;
using Core.Application.Features.ImportGoods.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Application.Services;
using Core.Application.Transforms;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.ImportGoods.Commands.CreateImportGoods
{
    public record CreateImportGoodsCommand : IBaseImportGoods, IRequest<Result<ImportGoodDto>>
    {
        public int? SupplierOrderId { get; set; }

        public string? ReceivingStaff { get; set; }

        public List<DetailImportGoodDto>? Details { get; set; }
    }

    public class CreateImportGoodsCommandHandler :
        CreateBaseCommandHandler<CreateImportGoodsValidator, CreateImportGoodsCommand, ImportGoodDto, SupplierOrder>
    {
        public CreateImportGoodsCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }

        protected override async Task<Result<ImportGoodDto>> Validator(CreateImportGoodsCommand request)
        {
            var validator = new CreateImportGoodsValidator(_context, request.SupplierOrderId);

            if (validator == null)
            {
                return Result<ImportGoodDto>.Failure(ValidatorTransform.ValidatorFailed(), StatusCodes.Status500InternalServerError);
            }

            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<ImportGoodDto>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }

            return Result<ImportGoodDto>.Success();
        }

        protected override async Task<SupplierOrder> Before(CreateImportGoodsCommand request)
        {
            var supplierOrder = await _context.SupplierOrders
                        .Include(x => x.Distributor)
                        .FirstOrDefaultAsync(x => x.Id == request.SupplierOrderId);

            var newSO = new SupplierOrder();
            newSO.CopyPropertiesFrom(supplierOrder);

            var productIds = request.Details.Select(d => d.ProductId).ToList();

            var detailsTotalPrice = await _context.DetailSupplierOrders
                .Where(x => x.SupplierOrderId == request.SupplierOrderId &&
                            productIds.Contains(x.ProductId))
                .ToListAsync();

            var totalPrice = detailsTotalPrice
                .Sum(detail => detail.Price * request.Details
                .FirstOrDefault(d => d.ProductId == detail.ProductId)?.ImportQuantity ?? 0);

            DateTime created = DateTime.Now;
            newSO.InternalCode = CommonService.InternalCodeGeneration("IMP_GOOD_", created);
            newSO.BookingDate = created;
            newSO.Total = totalPrice;
            newSO.Type = SupplierOrder.SupplierOrderType.Receive;
            newSO.Status = SupplierOrder.SupplierOrderStatus.Draft;
            newSO.ApproveStaffId = _currentUserService.StaffId;
            newSO.ReceivingStaff = request.ReceivingStaff;

            return newSO;
        }

        protected override async Task After(CreateImportGoodsCommand request, SupplierOrder entity, ImportGoodDto dto)
        {
            await _mediator.Publish(new AfterCreateImportGoodEvent(request, entity, dto));

            await _mediator.Send(new ChangeStatusImportGoodsCommand
            {
                SupplierOrderId = dto.Id,
                IsCancel = false,
            });
        }
    }
}
