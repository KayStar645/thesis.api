using Core.Application.Common.Interfaces;
using Core.Domain.Entities;
using static Core.Application.Transforms.Modules;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.Products.Commands.ChangeStatusProduct
{
    public class ChangeStatusProductValidator : AbstractValidator<ChangeStatusProductCommand>
    {
        public ChangeStatusProductValidator(ISupermarketDbContext pContext)
        {
            RuleFor(x => x.ProductId)
                .MustAsync(async (ProductId, token) =>
                {
                    return await pContext.Products
                            .AnyAsync(x => x.Id == ProductId &&
                                           x.Type == ProductType.Option);
                }).WithMessage("Id sản phẩm không hợp lệ!");

            RuleFor(x => x.Status)
                .MustAsync(async (request, status, token) =>
                {
                    var product = await pContext.Products.FindAsync(request.ProductId);

                    if (!((product.Status == ProductStatus.Draft &&
                       (status == ProductStatus.Active ||
                        status == ProductStatus.Pause ||
                        status == ProductStatus.Stop)) ||
                        (product.Status == ProductStatus.Active &&
                       (status == ProductStatus.Draft ||
                        status == ProductStatus.Pause ||
                        status == ProductStatus.Stop)) ||
                        (product.Status == ProductStatus.Pause &&
                       (status == ProductStatus.Draft ||
                        status == ProductStatus.Active ||
                        status == ProductStatus.Stop))))
                    {
                        return false;
                    }  

                    return true;
                }).WithMessage("Trạng thái thay đổi sản phẩm không hợp lệ!");
        }
    }
}
