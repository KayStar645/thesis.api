using Core.Application.Common.Interfaces;
using Core.Application.Exceptions;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.SupplierOrders.Commands.BaseSupplierOrder;
using Core.Application.Features.SupplierOrders.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.SupplierOrders.Commands.UpdateSupplierOrder
{
    public record UpdateSupplierOrderCommand : UpdateBaseCommand, IBaseSupplierOrder, IRequest<Result<SupplierOrderDto>>
    {
        public int? DistributorId { get; set; }

        public List<BaseSupplierOrder.DetailSupplierOrderDto>? Details { get; set; }
    }

    public class UpdateSupplierOrderCommandHandler :
        UpdateBaseCommandHandler<UpdateSupplierOrderValidator, UpdateSupplierOrderCommand, SupplierOrderDto, SupplierOrder>
    {
        public UpdateSupplierOrderCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }

        protected override async Task<SupplierOrder> Before(UpdateSupplierOrderCommand request)
        {
            SupplierOrder entity = await base.Before(request);

            if(entity.Status != SupplierOrder.SupplierOrderStatus.Draft)
            {
                throw new BadRequestException("Đơn hàng đã được đặt không được thay đổi!");
            }

            entity.BookingDate = DateTime.Now;
            entity.Total = request?.Details?.Sum(x => x.Price * x.Quantity);
            entity.ApproveStaffId = _currentUserService.StaffId;


            return entity;
        }

        protected override async Task After(UpdateSupplierOrderCommand request, SupplierOrder entity, SupplierOrderDto dto)
        {
            await _mediator.Publish(new AfterUpdateSupplierOrderEvent(request, entity, dto));
        }
    }
}
