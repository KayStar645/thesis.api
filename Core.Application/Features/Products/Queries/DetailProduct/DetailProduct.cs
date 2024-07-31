using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Features.Orders.Commands.BaseOrders;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Products.Queries.DetailProduct
{
    public record DetailProductCommand : DetailBaseCommand, IRequest<Result<ProductDto>>
    {
    }

    public class DetailProductCommandHandler :
        DetailBaseCommandHandler<DetailProductValidator, DetailProductCommand, ProductDto, Product>
    {
        private readonly IAmazonS3Service _amazonS3;
        public DetailProductCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService,
            IAmazonS3Service pAmazonS3Service)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
            _amazonS3 = pAmazonS3Service;
        }

        protected override IQueryable<Product> ApplyQuery(DetailProductCommand request, IQueryable<Product> query)
        {
            if (request.IsAllDetail)
            {
                query = query.Include(x => x.Category);
                query = query.Include(x => x.Parent);
            }

            if (_currentUserService.Type != CLAIMS_VALUES.TYPE_ADMIN &&
                _currentUserService.Type != CLAIMS_VALUES.TYPE_SUPER_ADMIN)
            {
                query = query.Where(x => x.Status == Product.ProductStatus.Active);
            }

            query = query.Where(x => x.Type == Product.ProductType.Option);

            return query;
        }

        protected override async Task<ProductDto> HandlerDtoAfterQuery(ProductDto dto)
        {
            var product = _mapper.Map<Product>(dto);

            (Promotion promo, decimal? priceDiscoutMax, int? group) =
                    await BaseOrderApplyPromotion.
                        ApplyPromotionForSingleProduct(_context, product);

            dto.NewPrice = product.Price - priceDiscoutMax;
            dto.PromotionDto = _mapper.Map<PromotionDto>(promo);

            return dto;
        }

    }
}
