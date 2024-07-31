using Core.Application.Common.Interfaces;
using Core.Application.Models.Common;
using Core.Application.Transforms;

namespace Core.Application.Features.Base.Commands.DeleteBase
{
    public class DeleteBaseCommandValidator<T> : AbstractValidator<T> where T : BaseDto
    {
        public DeleteBaseCommandValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Id));
        }
    }
}