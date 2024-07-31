using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Customers.Queries.ListCustomer
{
    public record ListCustomerCommand : ListBaseCommand, IRequest<PaginatedResult<List<CustomerDto>>>
    {
    }

    public class ListCustomerCommandHandler :
        ListBaseCommandHandler<ListCustomerValidator, ListCustomerCommand, CustomerDto, Customer>
    {
        public ListCustomerCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

        protected override IQueryable<Customer> ApplyQuery(ListCustomerCommand request, IQueryable<Customer> query)
        {
            if(request.IsAllDetail)
            {
                query = query.Include(x => x.User);
            }
            return query;
        }
    }
}
