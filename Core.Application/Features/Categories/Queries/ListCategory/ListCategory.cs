using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Sieve.Services;

namespace Core.Application.Features.Categories.Queries.ListCategory
{
    public record ListCategoryCommand : ListBaseCommand, IRequest<PaginatedResult<List<CategoryDto>>>
    {
    }

    public class ListCategoryCommandHandler :
        ListBaseCommandHandler<ListCategoryValidator, ListCategoryCommand, CategoryDto, Category>
    {
        private readonly IAmazonS3Service _amazonS3;
        public ListCategoryCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, ISieveProcessor pSieveProcessor,
            ICurrentUserService pCurrentUserService,
            IAmazonS3Service pAmazonS3Service)
            : base(pContext, pMapper, pSieveProcessor, pCurrentUserService)
        {
            _amazonS3 = pAmazonS3Service;
        }

        protected override IQueryable<Category> ApplyQuery(ListCategoryCommand request, IQueryable<Category> query)
        {
            if(request.IsAllDetail)
            {
                query = query.Include(x => x.Parent);
            }

            return query;
        }
    }
}
