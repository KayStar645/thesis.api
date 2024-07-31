using Core.Application.Common.Interfaces;
using Core.Application.Transforms;

namespace Core.Application.Features.Base.Commands.UpdateBase
{
    public class UpdateBaseCommandValidator : AbstractValidator<UpdateBaseCommand>
    {
        public UpdateBaseCommandValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Id));
        }
    }
}
