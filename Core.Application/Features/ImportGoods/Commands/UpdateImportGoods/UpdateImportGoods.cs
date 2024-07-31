using Core.Application.Common.Interfaces;
using Core.Application.Exceptions;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.ImportGoods.Commands.BaseImportGoods;
using Core.Application.Features.ImportGoods.Commands.CreateImportGoods;
using Core.Application.Features.ImportGoods.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Application.Transforms;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.ImportGoods.Commands.UpdateImportGoods
{
    public record UpdateImportGoodsCommand : UpdateBaseCommand, IBaseImportGoods, IRequest<Result<ImportGoodDto>>
    {
        public string? ReceivingStaff { get; set; }

        public List<DetailImportGoodDto>? Details { get; set; }
    }

    public class UpdateImportGoodsCommandHandler :
        UpdateBaseCommandHandler<UpdateImportGoodsValidator, UpdateImportGoodsCommand, ImportGoodDto, SupplierOrder>
    {
        public UpdateImportGoodsCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override async Task<Result<ImportGoodDto>> Validator(UpdateImportGoodsCommand request)
        {
            var sp = await _context.SupplierOrders.FindAsync(request.Id);
            var validator = new UpdateImportGoodsValidator(_context, sp.ParentId);

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

        protected override async Task<SupplierOrder> Before(UpdateImportGoodsCommand request)
        {
            SupplierOrder entity = await base.Before(request);

            if (entity.Status != SupplierOrder.SupplierOrderStatus.Draft)
            {
                throw new BadRequestException("Đơn hàng đã được đặt không được thay đổi!");
            }

            entity.BookingDate = DateTime.Now;

            var productIds = request.Details.Select(d => d.ProductId).ToList();

            var detailsTotalPrice = await _context.DetailSupplierOrders
                .Include(x => x.SupplierOrder)
                .Where(x => x.SupplierOrder.Id == request.Id &&
                            productIds.Contains(x.ProductId))
                .ToListAsync();

            var totalPrice = detailsTotalPrice
                .Sum(detail => detail.Price * request.Details
                .FirstOrDefault(d => d.ProductId == detail.ProductId)?.ImportQuantity ?? 0);

            entity.Total = totalPrice;
            entity.ReceivingStaff = request.ReceivingStaff;
            entity.ApproveStaffId = _currentUserService.StaffId;

            return entity;
        }

        protected override async Task After(UpdateImportGoodsCommand request, SupplierOrder entity, ImportGoodDto dto)
        {
            await _mediator.Publish(new AfterUpdateImportGoodsEvent(request, entity, dto));
        }
    }
}
