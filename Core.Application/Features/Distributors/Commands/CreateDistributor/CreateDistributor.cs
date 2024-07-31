using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Distributors.Commands.BaseDistributor;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Distributors.Commands.CreateDistributor
{
    public record CreateDistributorCommand : IBaseDistributor, IRequest<Result<DistributorDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }

    public class CreateDistributorCommandHandler :
        CreateBaseCommandHandler<CreateDistributorValidator, CreateDistributorCommand, DistributorDto, Distributor>
    {
        public CreateDistributorCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }
    }
}
