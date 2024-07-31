using Core.Application.Common.Interfaces;
using Core.Application.Exceptions;
using Core.Application.Extensions;
using Core.Application.Responses;
using Core.Application.Transforms;
using Core.Domain.Common;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Base.Commands.UpdateBase
{
    public record UpdateBaseCommand
    {
        public int? Id { get; set; }
    }

    public abstract class UpdateBaseCommandHandler<TValidator, TRequest, Dto, TEntity> : IRequestHandler<TRequest, Result<Dto>>
        where TValidator : AbstractValidator<TRequest>
        where TRequest : UpdateBaseCommand, IRequest<Result<Dto>>
        where TEntity : AuditableEntity
    {
        protected readonly ISupermarketDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;
        protected readonly ICurrentUserService _currentUserService;

        public UpdateBaseCommandHandler(ISupermarketDbContext pContext,
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

                var update = await this.Update(entity, cancellationToken);

                await this.After(request, entity, update.result.Data);

                return update.result;
            }
            catch (Exception ex)
            {
                return Result<Dto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        protected virtual async Task<Result<Dto>> Validator(TRequest request)
        {
            var validator = Activator.CreateInstance(typeof(TValidator), _context, request.Id) as TValidator;

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
            var findEntity = await _context.Set<TEntity>().FindAsync(request.Id);

            if (findEntity == null)
            {
                throw new BadRequestException(ValidatorTransform.NotExistsValue(Modules.Id,
                                request.Id.ToString()));
            }
            findEntity.CopyPropertiesFrom(request);

            return findEntity;
        }


        protected virtual async Task<(Result<Dto> result, TEntity? entity)> Update(TEntity entity, CancellationToken cancellationToken)
        {
            var newEntity = _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<Dto>(newEntity.Entity);

            return (Result<Dto>.Success(dto, StatusCodes.Status200OK), newEntity.Entity);
        }

        protected virtual async Task After(TRequest request, TEntity entity, Dto dto) { }
    }
}
