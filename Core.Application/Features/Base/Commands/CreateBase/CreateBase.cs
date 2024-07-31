using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Core.Application.Transforms;
using Core.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Base.Commands.CreateBase
{
    public abstract class CreateBaseCommandHandler<TValidator, TRequest, Dto, TEntity> : IRequestHandler<TRequest, Result<Dto>>
        where TValidator : AbstractValidator<TRequest>
        where TRequest : class, IRequest<Result<Dto>>
        where TEntity : AuditableEntity
    {
        protected readonly ISupermarketDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;
        protected readonly ICurrentUserService _currentUserService;

        public CreateBaseCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = pMediator;
            _currentUserService = currentUserService;
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

                var entity = await this.Before(request);
                entity.Id = 0;

                var create = await this.Create(entity, cancellationToken);

                await this.After(request, entity, create.result.Data);

                return create.result;
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

        protected virtual async Task<TEntity> Before(TRequest request)
        {
            return _mapper.Map<TEntity>(request);
        }

        protected virtual async Task<(Result<Dto> result, TEntity entity)> Create(TEntity entity, CancellationToken cancellationToken)
        {
            var newEntity = await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<Dto>(newEntity.Entity);

            return (Result<Dto>.Success(dto, StatusCodes.Status201Created), newEntity.Entity);
        }

        protected virtual async Task After(TRequest request, TEntity entity, Dto dto) { }
    }
}
