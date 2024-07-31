using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.DeleteBase;
using Core.Application.Models.Common;
using Core.Domain.Entities;

namespace Core.Application.Features.Payments.Commands.DeletePayment
{
    public record DeletePaymentCommand : BaseDto, IRequest<Unit>
    {
    }

    public class DeletePaymentCommandHandler :
        DeleteBaseCommandHandler<DeleteBaseCommandValidator<DeletePaymentCommand>, DeletePaymentCommand, Payment>
    {
        public DeletePaymentCommandHandler(ISupermarketDbContext pContext, IMapper pMapper, IMediator pMediator) : base(pContext, pMapper, pMediator) { }
    }
}