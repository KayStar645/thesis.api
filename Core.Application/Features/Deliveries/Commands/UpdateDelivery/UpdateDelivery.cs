using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Exceptions;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Deliveries.Commands.BaseDelivery;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Deliveries.Commands.UpdateDelivery
{
    public record UpdateDeliveryCommand : UpdateBaseCommand, IBaseDelivery, IRequest<Result<DeliveryDto>>
    {
        public int? OrderId { get; set; }

        public string? From { get; set; }

        public string? To { get; set; }

        public decimal? TransportFee { get; set; }
    }

    public class UpdateDeliveryCommandHandler :
        UpdateBaseCommandHandler<UpdateDeliveryValidator, UpdateDeliveryCommand, DeliveryDto, Delivery>
    {
        public UpdateDeliveryCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override async Task<Delivery> Before(UpdateDeliveryCommand request)
        {
            var entity = await base.Before(request);

            entity.DateSent = DateTime.Now;
            entity.PackingStaffId = _currentUserService.StaffId;

            return entity;
        }
    }

}
