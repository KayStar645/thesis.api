using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Payments.Queries.ListPayment
{
    public record ListPaymentCommand : ListBaseCommand, IRequest<PaginatedResult<List<PaymentDto>>>
    {
    }

    public class ListPaymentCommandHandler :
        ListBaseCommandHandler<ListPaymentValidator, ListPaymentCommand, PaymentDto, Payment>
    {
        public ListPaymentCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

    }
}
