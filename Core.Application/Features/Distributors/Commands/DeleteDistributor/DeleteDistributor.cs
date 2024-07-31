using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.DeleteBase;
using Core.Application.Models.Common;
using Core.Domain.Entities;
using Unit = MediatR.Unit;

namespace Core.Application.Features.Distributors.Commands.DeleteDistributor
{
    public record DeleteDistributorCommand : BaseDto, IRequest<Unit>
    {
    }

    public class DeleteDistributorCommandHandler :
        DeleteBaseCommandHandler<DeleteBaseCommandValidator<DeleteDistributorCommand>, DeleteDistributorCommand, Distributor>
    {
        public DeleteDistributorCommandHandler(ISupermarketDbContext pContext, IMapper pMapper, IMediator pMediator) : base(pContext, pMapper, pMediator) { }
    }
}