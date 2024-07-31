using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.DeleteBase;
using Core.Application.Features.StaffPositions.Events;
using Core.Application.Models.Common;
using Core.Domain.Entities;

namespace Core.Application.Features.StaffPositions.Commands.DeleteStaffPosition
{
    public record DeleteStaffPositionCommand : BaseDto, IRequest<Unit>
    {
    }

    public class DeleteStaffPositionCommandHandler :
        DeleteBaseCommandHandler<DeleteBaseCommandValidator<DeleteStaffPositionCommand>, DeleteStaffPositionCommand, StaffPosition>
    {
        public DeleteStaffPositionCommandHandler(ISupermarketDbContext pContext, IMapper pMapper, IMediator pMediator) :
            base(pContext, pMapper, pMediator) { }

        protected override async Task Delete(DeleteStaffPositionCommand request, CancellationToken cancellationToken)
        {
            await base.Delete(request, cancellationToken);

            await _mediator.Publish(new AfterCreateOrUpdateOrDeleteStaffPositionEvent(request.Id, null, null));
        }
    }
}