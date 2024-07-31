using Core.Application.Common.Interfaces;
using Core.Application.Exceptions;
using Core.Application.Models.Common;
using Core.Application.Transforms;
using Core.Domain.Common;

namespace Core.Application.Features.Base.Commands.DeleteBase
{
    public abstract class DeleteBaseCommandHandler<TValidator, TRequest, TEntity> : IRequestHandler<TRequest, Unit>
        where TValidator : AbstractValidator<TRequest>
        where TRequest : BaseDto, IRequest<Unit>
        where TEntity : AuditableEntity
    {
        protected readonly ISupermarketDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;

        public DeleteBaseCommandHandler(ISupermarketDbContext pContext, IMapper pMapper, IMediator pMediator)
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = pMediator;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            try
            {
                this.Validator(request);

                await this.Delete(request, cancellationToken);

                return Unit.Value;
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(string.Join(",", ex.Message));
            }
            catch (BadRequestException ex)
            {
                throw new BadRequestException(string.Join(",", ex.Message));
            }
            catch (Exception ex)
            {
                throw new Exception(string.Join(",", ex.Message));
            }
        }

        protected virtual async void Validator(TRequest request)
        {
            var validator = Activator.CreateInstance(typeof(TValidator), _context) as TValidator;

            if (validator == null)
            {
                throw new Exception(ValidatorTransform.ValidatorFailed());
            }

            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                throw new BadRequestException(string.Join(",", errorMessages));
            }
        }

        protected virtual async Task Delete(TRequest request, CancellationToken cancellationToken)
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(x => x.Id == request.Id && x.IsDeleted == false);

            if (entity == null)
                throw new NotFoundException(Modules.Id, request.Id.ToString());

            _context.Set<TEntity>().Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
