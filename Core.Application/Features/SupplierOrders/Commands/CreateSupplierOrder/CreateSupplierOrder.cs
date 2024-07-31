using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.SupplierOrders.Commands.BaseSupplierOrder;
using Core.Application.Features.SupplierOrders.Commands.CreateOrderSupplier;
using Core.Application.Features.SupplierOrders.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Application.Services;
using Core.Domain.Entities;

namespace Core.Application.Features.SupplierOrders.Commands.CreateSupplierOrder
{
    public record CreateSupplierOrderCommand : IBaseSupplierOrder, IRequest<Result<SupplierOrderDto>>
    {
        public int? DistributorId { get; set; }

        public List<BaseSupplierOrder.DetailSupplierOrderDto>? Details { get; set; }
    }

    public class CreateSupplierOrderCommandHandler :
        CreateBaseCommandHandler<CreateSupplierOrderValidator, CreateSupplierOrderCommand, SupplierOrderDto, SupplierOrder>
    {
        public CreateSupplierOrderCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }

        protected override async Task<SupplierOrder> Before(CreateSupplierOrderCommand request)
        {
            DateTime created = DateTime.Now;
            var supplierOrder = _mapper.Map<SupplierOrder>(request);

            supplierOrder.InternalCode = CommonService.InternalCodeGeneration("SUP_ORDER_", created);
            supplierOrder.BookingDate = created;
            supplierOrder.Total = request?.Details?.Sum(x => x.Price * x.Quantity);
            supplierOrder.Type = SupplierOrder.SupplierOrderType.Order;
            supplierOrder.Status = SupplierOrder.SupplierOrderStatus.Draft;
            supplierOrder.ApproveStaffId = _currentUserService.StaffId;

            return supplierOrder;
        }

        protected override async Task After(CreateSupplierOrderCommand request, SupplierOrder entity, SupplierOrderDto dto)
        {
            await _mediator.Publish(new AfterCreateSupplierOrderEvent(request, entity, dto));
        }
    }
}
