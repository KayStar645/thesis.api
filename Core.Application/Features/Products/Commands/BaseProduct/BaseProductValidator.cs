using Core.Application.Common.Interfaces;
using Core.Application.Transforms;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.Products.Commands.BaseProduct
{
    public class BaseProductValidator : AbstractValidator<IBaseProduct>
    {
        public BaseProductValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            RuleFor(x => x.InternalCode)
               .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.InternalCode))
               .MinimumLength(Modules.InternalCodeMin)
               .WithMessage(ValidatorTransform.MinimumLength(Modules.InternalCode, Modules.InternalCodeMin))
               .MaximumLength(Modules.InternalCodeMax)
               .WithMessage(ValidatorTransform.MinimumLength(Modules.InternalCode, Modules.InternalCodeMax))
               .MustAsync(async (internalCode, token) =>
               {
                   bool exists;

                   if (pCurrentId == null)
                   {
                       exists = await pContext.Products
                            .AnyAsync(x => x.InternalCode == internalCode &&
                                           x.IsDeleted == false &&
                                           x.Type == ProductType.Option);
                   }
                   else
                   {
                       exists = await pContext.Products
                            .AnyAsync(x => x.InternalCode == internalCode &&
                                           x.Id != pCurrentId &&
                                           x.IsDeleted == false &&
                                           x.Type == ProductType.Option);
                   }

                   return !exists;
               }).WithMessage(ValidatorTransform.Exists(Modules.InternalCode));

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Name))
                .MinimumLength(Modules.NameMin)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.Name, Modules.NameMin))
                .MaximumLength(Modules.NameMax)
                .WithMessage(ValidatorTransform.MinimumLength(Modules.Name, Modules.NameMax))
                .MustAsync(async (name, token) =>
                {
                    bool exists;

                    if (pCurrentId == null)
                    {
                        exists = await pContext.Products
                                .AnyAsync(x => x.Name == name &&
                                               x.IsDeleted == false &&
                                           x.Type == ProductType.Option);
                    }
                    else
                    {
                        exists = await pContext.Products
                                .AnyAsync(x => x.Name == name &&
                                               x.Id != pCurrentId &&
                                               x.IsDeleted == false &&
                                           x.Type == ProductType.Option);
                    }
                    return !exists;
                }).WithMessage(ValidatorTransform.Exists(Modules.Name));

            RuleFor(x => x.CategoryId)
                .MustAsync(async (categoryId, token) =>
                {
                    return categoryId == null ||
                           await pContext.Categories
                           .AnyAsync(x => x.Id == categoryId &&
                                          x.IsDeleted == false);
                }).WithMessage(ValidatorTransform.NotExists(Modules.Product.CategoryId));

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Product.Price))
                .Must(x => Modules.Product.MinPrice < x)
                .WithMessage(ValidatorTransform.MustValueMin(Modules.Product.Price, Modules.Product.MinPrice));

            RuleFor(x => x.Describes)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Describes))
                .MaximumLength(Modules.MaxDescribes)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Describes, Modules.MaxDescribes));

            RuleFor(x => x.Feature)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Product.Feature))
                .MaximumLength(Modules.MaxDescribes)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Product.Feature, Modules.MaxDescribes));

            RuleFor(x => x.Specifications)
                .NotEmpty().WithMessage(ValidatorTransform.Required(Modules.Product.Specifications))
                .MaximumLength(Modules.MaxDescribes)
                .WithMessage(ValidatorTransform.MaximumLength(Modules.Product.Specifications, Modules.MaxDescribes));

            var enumValues = Enum.GetValues(typeof(ProductStatus))
                    .Cast<ProductStatus>()
                    .Select(v => v.ToString())
                    .ToArray();
        }
    }
}
