using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetListBase;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Features.Roles.Queries.ListRoleWithPermission;
using Core.Application.Models;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using Sieve.Models;
using Sieve.Services;

namespace Core.Application.Features.Roles.Queries.ListRoleWithPermissionWithPermission
{
    public record ListRoleWithPermissionCommand : ListBaseCommand, IRequest<PaginatedResult<List<RoleDto>>>
    {
        
    }

    public class ListRoleWithPermissionCommandHandler : IRequestHandler<ListRoleWithPermissionCommand, PaginatedResult<List<RoleDto>>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMapper _mapper;
        protected readonly IMediator _mediator;
        protected readonly ISieveProcessor _sieveProcessor;

        public ListRoleWithPermissionCommandHandler(
            ISupermarketDbContext pContext,
            IMapper pMapper,
            IMediator mediator,
            ISieveProcessor pSieveProcessor
            )
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = mediator;
            _sieveProcessor = pSieveProcessor;
        }

        public async Task<PaginatedResult<List<RoleDto>>> Handle(ListRoleWithPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ListRoleWithPermissionValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return PaginatedResult<List<RoleDto>>.Failure(StatusCodes.Status400BadRequest, errorMessages);
                }
                var query = _context.Permissions.Select(x => x.Name).AsQueryable();

                request.PageSize = 1000;
                var sieve = _mapper.Map<SieveModel>(request);

                int totalCount = await PaginatedResultBase.CountApplySieveAsync(_sieveProcessor, sieve, query);

                query = _sieveProcessor.Apply(sieve, query);

                var results = await query.ToListAsync();

                var groupedPermissions = results
                    .GroupBy(p => p.Split('.')[0])
                    .Select(g => new RoleDto
                    {
                        Name = g.Key,
                        Permissions = g.Select(p => p.Split('.')[1]).ToList()
                    })
                    .ToList();

                // Phân trang
                var mapResults = _mapper.Map<List<RoleDto>>(groupedPermissions);

                return PaginatedResult<List<RoleDto>>.Success(mapResults, totalCount, request.Page, request.PageSize);
            }
            catch (Exception ex)
            {
                return PaginatedResult<List<RoleDto>>.Failure(StatusCodes.Status500InternalServerError,
                    new List<string> { ex.Message });
            }
        }
    }
}
