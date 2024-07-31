using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.StaffPositions.Commands.BaseStaffPosition;
using Core.Application.Features.StaffPositions.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.StaffPositions.Commands.UpdateStaffPosition
{
    public record UpdateStaffPositionCommand : UpdateBaseCommand, IBaseStaffPosition, IRequest<Result<StaffPositionDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public string? Describes { get; set; }

        public List<int?>? Roles { get; set; }
    }

    public class UpdateStaffPositionCommandValidator :
        UpdateBaseCommandHandler<UpdateStaffPositionValidator, UpdateStaffPositionCommand, StaffPositionDto, StaffPosition>
    {
        public UpdateStaffPositionCommandValidator(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override async Task After(UpdateStaffPositionCommand request, StaffPosition entity, StaffPositionDto dto)
        {
            await _mediator.Publish(new AfterCreateOrUpdateOrDeleteStaffPositionEvent(request.Id, request.InternalCode, request.Roles));
        }
    }
}
