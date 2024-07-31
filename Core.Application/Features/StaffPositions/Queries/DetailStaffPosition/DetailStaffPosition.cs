using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.StaffPositions.Queries.DetailStaffPosition
{
    public record DetailStaffPositionCommand : DetailBaseCommand, IRequest<Result<StaffPositionDto>>
    {
    }

    public class DetailStaffPositionCommandHandler :
        DetailBaseCommandHandler<DetailStaffPositionValidator, DetailStaffPositionCommand, StaffPositionDto, StaffPosition>
    {
        public DetailStaffPositionCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override async Task<StaffPositionDto> HandlerDtoAfterQuery(StaffPositionDto dto)
        {
            dto.Roles = await _context.StaffPositionHasRoles
                .Where(x => x.StaffPositionId == dto.Id)
                .Select(x => x.RoleId)
                .ToListAsync();

            return dto;
        }

    }
}
