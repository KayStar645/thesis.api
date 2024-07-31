using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Customers.Commands.BaseCustomer;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Customers.Commands.UpdateCustomer
{
    public record UpdateCustomerCommand : UpdateBaseCommand, IBaseCustomer, IRequest<Result<CustomerDto>>
    {
        public string? Name { get; set; }

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public string? Gender { get; set; }
    }

    public class UpdateCustomerCommandHandler :
        UpdateBaseCommandHandler<UpdateCustomerValidator, UpdateCustomerCommand, CustomerDto, Customer>
    {
        public UpdateCustomerCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }
    }

}
