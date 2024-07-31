using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Payments.Commands.BasePayment;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Payments.Commands.CreatePayment
{
    public record CreatePaymentCommand : IBasePayment, IRequest<Result<PaymentDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }
    }

    public class CreatePaymentCommandHandler :
        CreateBaseCommandHandler<CreatePaymentValidator, CreatePaymentCommand, PaymentDto, Payment>
    {
        public CreatePaymentCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }
    }
}
