using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http.Features;

namespace Core.Application.Features.Customers.Queries.DetailCustomer
{
    public record GetDetailCustomerCommand : DetailBaseCommand, IRequest<Result<CustomerDto>>
    {
    }

    public class GetDetailCustomerCommandHandler :
        DetailBaseCommandHandler<DetailCustomerValidator, GetDetailCustomerCommand, CustomerDto, Customer>
    {
        public GetDetailCustomerCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override IQueryable<Customer> ApplyQuery(GetDetailCustomerCommand request, IQueryable<Customer> query)
        {
            if(request.IsAllDetail)
            {
                query = query.Include(x => x.User);
            }
            return query;
        }

    }
}
