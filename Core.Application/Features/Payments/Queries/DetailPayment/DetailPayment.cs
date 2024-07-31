using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Payments.Queries.DetailPayment
{
    public record DetailPaymentCommand : DetailBaseCommand, IRequest<Result<PaymentDto>>
    {
    }

    public class DetailPaymentCommandHandler :
        DetailBaseCommandHandler<DetailPaymentValidator, DetailPaymentCommand, PaymentDto, Payment>
    {
        public DetailPaymentCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

    }
}
