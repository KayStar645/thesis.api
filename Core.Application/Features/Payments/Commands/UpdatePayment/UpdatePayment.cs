using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Payments.Commands.BasePayment;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Payments.Commands.UpdatePayment
{
    public record UpdatePaymentCommand : UpdateBaseCommand, IBasePayment, IRequest<Result<PaymentDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }
    }

    public class UpdatePaymentCommandValidator :
        UpdateBaseCommandHandler<UpdatePaymentValidator, UpdatePaymentCommand, PaymentDto, Payment>
    {
        public UpdatePaymentCommandValidator(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }
    }
}
