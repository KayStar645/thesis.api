using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.DeleteBase;
using Core.Application.Models.Common;
using Core.Domain.Entities;
using Unit = MediatR.Unit;

namespace Core.Application.Features.Staffs.Commands.DeleteStaff
{
    public record DeleteStaffCommand : BaseDto, IRequest<Unit>
    {
    }

    public class DeleteStaffCommandHandler : 
        DeleteBaseCommandHandler<DeleteBaseCommandValidator<DeleteStaffCommand>, DeleteStaffCommand, Staff>
    {
        public DeleteStaffCommandHandler(ISupermarketDbContext pContext, IMapper pMapper, IMediator pMediator) : base(pContext, pMapper, pMediator) { }
    }
}