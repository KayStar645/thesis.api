using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Products.Commands.BaseProduct;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.Products.Commands.CreateProduct
{
    public record CreateProductCommand : IBaseProduct, IRequest<Result<ProductDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public List<string>? Images { get; set; }

        public decimal? Price { get; set; }

        public string? Describes { get; set; }

        public string? Feature { get; set; }

        public string? Specifications { get; set; }

        // Khoá ngoại

        public int? CategoryId { get; set; }
    }

    public class CreateProductCommandHandler :
        CreateBaseCommandHandler<CreateProductValidator, CreateProductCommand, ProductDto, Product>
    {
        public CreateProductCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }

        protected override async Task<Product> Before(CreateProductCommand request)
        {
            var product = _mapper.Map<Product>(request);
            product.Type = ProductType.Option;
            product.Status = ProductStatus.Draft;
            product.Quantity = 0;

            return product;
        }
    }
}
