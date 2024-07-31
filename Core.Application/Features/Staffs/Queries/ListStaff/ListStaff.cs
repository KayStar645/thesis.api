using Amazon.S3;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Staffs.Queries.ListStaff
{
    public record ListStaffCommand : ListBaseCommand, IRequest<PaginatedResult<List<StaffDto>>>
    {
    }

    public class ListStaffCommandHandler : 
        ListBaseCommandHandler<ListStaffValidator, ListStaffCommand, StaffDto, Staff>
    {
        private readonly IAmazonS3Service _amazonS3;
        public ListStaffCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService, IAmazonS3Service amazonS3)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
            _amazonS3 = amazonS3;
        }
    }
}
