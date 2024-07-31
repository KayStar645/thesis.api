using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Exceptions;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Deliveries.Commands.BaseDelivery;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Application.Services;
using Core.Domain.Entities;
using static Core.Domain.Entities.Delivery;

namespace Core.Application.Features.Deliveries.Commands.CreateDelivery
{
    public record CreateDeliveryCommand : IBaseDelivery, IRequest<Result<DeliveryDto>>
    {
        public int? OrderId { get; set; }

        public string? From { get; set; }

        public string? To { get; set; }

        public decimal? TransportFee { get; set; }
    }

    public class CreateDeliveryCommandHandler :
        CreateBaseCommandHandler<CreateDeliveryValidator, CreateDeliveryCommand, DeliveryDto, Delivery>
    {
        public CreateDeliveryCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }

        protected override async Task<Delivery> Before(CreateDeliveryCommand request)
        {
            var entity = await base.Before(request);

            var create = DateTime.Now;
            entity.InternalCode = CommonService.InternalCodeGeneration("ORDER", create);
            entity.DateSent = create;
            entity.Status = DeliveryStatus.Prepare;
            entity.PackingStaffId = _currentUserService.StaffId;

            return entity;
        }
    }
}
