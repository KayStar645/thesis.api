using Core.Application.Common.Interfaces;
using Core.Application.Extensions;
using Core.Application.Features.Base.Queries.GetListBase;
using Core.Application.Responses;
using Core.Application.Transforms;
using Core.Domain.Common;
using Microsoft.AspNetCore.Http;
using Sieve.Models;
using Sieve.Services;

namespace Core.Application.Features.Base.Queries.ListBase
{
    public record ListBaseCommand
    {
        public string? Filters { get; set; }
        public string? Sorts { get; set; }
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 100;
        public bool IsAllDetail { get; set; }
    }

    public abstract class ListBaseCommandHandler<TValidator, TRequest, Dto, TEntity> : IRequestHandler<TRequest, PaginatedResult<List<Dto>>>
        where TValidator : AbstractValidator<TRequest>
        where TRequest : ListBaseCommand, IRequest<PaginatedResult<List<Dto>>>
        where TEntity : AuditableEntity
    {
        protected readonly ISupermarketDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly ISieveProcessor _sieveProcessor;
        protected readonly ICurrentUserService _currentUserService;

        public ListBaseCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mapper = pMapper;
            _sieveProcessor = pSieveProcessor;
            _currentUserService = currentUserService;
        }

        public virtual async Task<PaginatedResult<List<Dto>>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await this.Validator(request);

                if (validator.Succeeded == false)
                {
                    return validator;
                }

                var list = await this.List(request, cancellationToken);

                return list;
            }
            catch (Exception ex)
            {
                return PaginatedResult<List<Dto>>
                    .Failure(StatusCodes.Status500InternalServerError,
                        new List<string> { ex.Message });
            }
        }

        protected virtual async Task<PaginatedResult<List<Dto>>> Validator(TRequest request)
        {
            var validator = Activator.CreateInstance(typeof(TValidator), _context) as TValidator;

            if (validator == null)
            {
                return PaginatedResult<List<Dto>>
                    .Failure(StatusCodes.Status500InternalServerError, new List<string> { ValidatorTransform.ValidatorFailed() });
            }

            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return PaginatedResult<List<Dto>>
                    .Failure(StatusCodes.Status400BadRequest, errorMessages);
            }

            return PaginatedResult<List<Dto>>.Success(new List<Dto>(), 0);
        }

        protected virtual async Task<PaginatedResult<List<Dto>>> List(TRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Set<TEntity>().FilterDeleted();

            var sieve = _mapper.Map<SieveModel>(request);

            query = ApplyQuery(request, query);

            int totalCount = await PaginatedResultBase.CountApplySieveAsync(_sieveProcessor, sieve, query);

            query = _sieveProcessor.Apply(sieve, query);

            var results = await query.ToListAsync();

            var mapResults = _mapper.Map<List<Dto>>(results);

            var resultListDto = await HandlerDtoAfterQuery(request, mapResults);

            return PaginatedResult<List<Dto>>.Success(resultListDto, totalCount, request.Page, request.PageSize);
        }

        protected virtual IQueryable<TEntity> ApplyQuery(TRequest request, IQueryable<TEntity> query) {  return query; }

        protected virtual async Task<List<Dto>> HandlerDtoAfterQuery(TRequest request, List<Dto> listDto)
        {
            return listDto;
        }
    }
}
