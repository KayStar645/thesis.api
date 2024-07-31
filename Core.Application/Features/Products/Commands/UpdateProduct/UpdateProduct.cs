using Core.Application.Common.Interfaces;
using Core.Application.Exceptions;
using Core.Application.Extensions;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Products.Commands.BaseProduct;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Application.Transforms;
using Core.Domain.Entities;
using System.Security.Principal;

namespace Core.Application.Features.Products.Commands.UpdateProduct
{
    public record UpdateProductCommand : UpdateBaseCommand, IBaseProduct, IRequest<Result<ProductDto>>
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

    public class UpdateProductCommandHandler :
        UpdateBaseCommandHandler<UpdateProductValidator, UpdateProductCommand, ProductDto, Product>
    {
        public UpdateProductCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override async Task<Product> Before(UpdateProductCommand request)
        {
            var findEntity = await base.Before(request);

            findEntity.Images = _mapper.Map<string>(request.Images);
            return findEntity;
        }
    }
}
