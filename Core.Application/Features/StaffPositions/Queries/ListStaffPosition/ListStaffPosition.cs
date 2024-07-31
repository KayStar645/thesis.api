using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.StaffPositions.Queries.ListStaffPosition
{
    public record ListStaffPositionCommand : ListBaseCommand, IRequest<PaginatedResult<List<StaffPositionDto>>>
    {
    }

    public class ListStaffPositionCommandHandler :
        ListBaseCommandHandler<ListStaffPositionValidator, ListStaffPositionCommand, StaffPositionDto, StaffPosition>
    {
        public ListStaffPositionCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
        }

    }
}
