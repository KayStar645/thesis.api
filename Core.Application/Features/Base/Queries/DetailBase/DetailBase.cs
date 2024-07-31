using Core.Application.Common.Interfaces;
using Core.Application.Extensions;
using Core.Application.Responses;
using Core.Application.Transforms;
using Core.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Base.Queries.GetRequestBase
{
    public record DetailBaseCommand
    {
        public int Id { get; set; }

        public bool IsAllDetail { get; set; }
    }

    public abstract class DetailBaseCommandHandler<TValidator, TRequest, Dto, TEntity> : IRequestHandler<TRequest, Result<Dto>>
        where TValidator : AbstractValidator<TRequest>
        where TRequest : DetailBaseCommand, IRequest<Result<Dto>>
        where TEntity : AuditableEntity
    {
        protected readonly ISupermarketDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;
        protected readonly ICurrentUserService _currentUserService;

        public DetailBaseCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = pMediator;
            _currentUserService = pCurrentUserService;
        }

        public virtual async Task<Result<Dto>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = await this.Validator(request);

                if (validator.Succeeded == false)
                {
                    return validator;
                }

                var detail = await this.Detail(request, cancellationToken);

                return detail.result;
            }
            catch (Exception ex)
            {
                return Result<Dto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        protected virtual async Task<Result<Dto>> Validator(TRequest request)
        {
            var validator = Activator.CreateInstance(typeof(TValidator), _context) as TValidator;

            if (validator == null)
            {
                return Result<Dto>.Failure(ValidatorTransform.ValidatorFailed(), StatusCodes.Status500InternalServerError);
            }

            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<Dto>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }

            return Result<Dto>.Success();
        }

        protected virtual async Task<(Result<Dto> result, TEntity? entity)> Detail(TRequest request, CancellationToken cancellationToken)
        {
            var query = _context.Set<TEntity>().FilterDeleted().Where(x => x.Id == request.Id);

            query = ApplyQuery(request, query);

            var findEntity = await query.SingleOrDefaultAsync();


            if (findEntity is null)
            {
                return (Result<Dto>.Failure(
                    ValidatorTransform.NotExistsValue(Modules.Id, request.Id.ToString()),
                    StatusCodes.Status404NotFound), null);
            }

            var dto = _mapper.Map<Dto>(findEntity);

            var resultDto = await HandlerDtoAfterQuery(dto);

            return (Result<Dto>.Success(resultDto, StatusCodes.Status200OK), findEntity);
        }

        protected virtual IQueryable<TEntity> ApplyQuery(TRequest request, IQueryable<TEntity> query) { return query; }

        protected virtual async Task<Dto> HandlerDtoAfterQuery(Dto dto)
        {
            return dto;
        }
    }
}
