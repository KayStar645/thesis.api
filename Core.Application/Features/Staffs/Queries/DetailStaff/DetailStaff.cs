using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Staffs.Queries.DetailStaff
{
    public record DetailStaffCommand : DetailBaseCommand, IRequest<Result<StaffDto>>
    {
    }

    public class DetailStaffCommandHandler : 
        DetailBaseCommandHandler<DetailStaffValidator, DetailStaffCommand, StaffDto, Staff>
    {
        private readonly IAmazonS3Service _amazonS3;
        public DetailStaffCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService, IAmazonS3Service amazonS3)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
            _amazonS3 = amazonS3;
        }

        protected override IQueryable<Staff> ApplyQuery(DetailStaffCommand request, IQueryable<Staff> query)
        {
            query = query.Include(x => x.Position);

            return query;
        }
    }
}
