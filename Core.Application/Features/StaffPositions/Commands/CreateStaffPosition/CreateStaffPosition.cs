using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.StaffPositions.Commands.BaseStaffPosition;
using Core.Application.Features.StaffPositions.Events;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.StaffPositions.Commands.CreateStaffPosition
{
    public record CreateStaffPositionCommand : IBaseStaffPosition, IRequest<Result<StaffPositionDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public string? Describes { get; set; }

        public List<int?>? Roles { get; set; }
    }

    public class CreateStaffPositionCommandHandler :
        CreateBaseCommandHandler<CreateStaffPositionValidator, CreateStaffPositionCommand, StaffPositionDto, StaffPosition>
    {
        public CreateStaffPositionCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override async Task After(CreateStaffPositionCommand request, StaffPosition entity, StaffPositionDto dto)
        {
            await _mediator.Publish(new AfterCreateOrUpdateOrDeleteStaffPositionEvent(null, request.InternalCode, request.Roles));
        }
    }
}
