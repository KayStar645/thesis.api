using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Features.Orders.Commands.BaseOrders;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Products.Queries.ListProduct
{
    public record ListProductCommand : ListBaseCommand, IRequest<PaginatedResult<List<ProductDto>>>
    {
    }

    public class ListProductCommandHandler :
        ListBaseCommandHandler<ListProductValidator, ListProductCommand, ProductDto, Product>
    {
        private readonly IAmazonS3Service _amazonS3;
        public ListProductCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService, IAmazonS3Service amazonS3)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
            _amazonS3 = amazonS3;
        }

        protected override IQueryable<Product> ApplyQuery(ListProductCommand request, IQueryable<Product> query)
        {
            if (_currentUserService.Type != CLAIMS_VALUES.TYPE_ADMIN &&
                _currentUserService.Type != CLAIMS_VALUES.TYPE_SUPER_ADMIN)
            {
                query = query.Where(x => x.Status == Product.ProductStatus.Active);
            }

            if (request.IsAllDetail)
            {
                query = query.Include(x => x.Category);
                query = query.Include(x => x.Parent);
            }

            query = query.Where(x => x.Type == Product.ProductType.Option);

            return query;
        }

        protected override async Task<List<ProductDto>> HandlerDtoAfterQuery(ListProductCommand request, List<ProductDto> listDto)
        {
            if(request.IsAllDetail)
            {
                for (int i = 0; i < listDto.Count; i++)
                {
                    var product = _mapper.Map<Product>(listDto[i]);

                    (Promotion promo, decimal? priceDiscoutMax, int? group) =
                            await BaseOrderApplyPromotion.
                                ApplyPromotionForSingleProduct(_context, product);

                    listDto[i].NewPrice = product.Price - priceDiscoutMax;
                    listDto[i].PromotionDto = _mapper.Map<PromotionDto>(promo);
                }
            }

            return listDto;
        }
    }
}
