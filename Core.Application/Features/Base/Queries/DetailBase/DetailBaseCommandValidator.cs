using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Transforms;

namespace Core.Application.Features.Base.Queries.GetDetailBase
{
    public class DetailBaseCommandValidator : AbstractValidator<DetailBaseCommand>
    {
        public DetailBaseCommandValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Id));
        }
    }
}