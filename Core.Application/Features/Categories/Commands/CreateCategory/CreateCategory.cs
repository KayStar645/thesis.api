using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.CreateBase;
using Core.Application.Features.Categories.Commands.BaseCategory;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Categories.Commands.CreateCategory
{
    public record CreateCategoryCommand : IBaseCategory, IRequest<Result<CategoryDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }

        public string? Icon { get; set; }

        public int? ParentId { get; set; }
    }

    public class CreateCategoryCommandHandler :
        CreateBaseCommandHandler<CreateCategoryValidator, CreateCategoryCommand, CategoryDto, Category>
    {
        public CreateCategoryCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {

        }
    }
}
