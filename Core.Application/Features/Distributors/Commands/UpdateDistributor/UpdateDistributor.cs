using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Distributors.Commands.BaseDistributor;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Distributors.Commands.UpdateDistributor
{
    public record UpdateDistributorCommand : UpdateBaseCommand, IBaseDistributor, IRequest<Result<DistributorDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }

    public class UpdateDistributorCommandValidator :
        UpdateBaseCommandHandler<UpdateDistributorValidator, UpdateDistributorCommand, DistributorDto, Distributor>
    {
        public UpdateDistributorCommandValidator(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }
    }
}
