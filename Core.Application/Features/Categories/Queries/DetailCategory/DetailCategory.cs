using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Categories.Queries.DetailCategory
{
    public record DetailCategoryCommand : DetailBaseCommand, IRequest<Result<CategoryDto>>
    {
    }

    public class GetDetailCategoryCommandHandler :
        DetailBaseCommandHandler<DetailCategoryValidator, DetailCategoryCommand, CategoryDto, Category>
    {
        private readonly IAmazonS3Service _amazonS3;
        public GetDetailCategoryCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService,
            IAmazonS3Service pAmazonS3Service)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
            _amazonS3 = pAmazonS3Service;
        }

        protected override IQueryable<Category> ApplyQuery(DetailCategoryCommand request, IQueryable<Category> query)
        {
            if (request.IsAllDetail)
            {
                query = query.Include(x => x.Parent);
            }

            return query;
        }

    }
}
