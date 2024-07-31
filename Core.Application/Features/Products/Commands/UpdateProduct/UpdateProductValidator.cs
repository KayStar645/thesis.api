using Core.Application.Common.Interfaces;
using Core.Application.Features.Products.Commands.BaseProduct;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator(ISupermarketDbContext pContext, int? pCurrentId = null)
        {
            Include(new BaseProductValidator(pContext, pCurrentId));

            RuleFor(x => x.Id)
                .MustAsync(async (id, token) =>
                {
                    return await pContext.Products
                    .AnyAsync(x => x.Id == id && x.Status == ProductStatus.Draft);
                }).WithMessage("Chỉ sửa được thông tin sản phẩm khi ở trạng thái nháp!");
        }
    }
}
