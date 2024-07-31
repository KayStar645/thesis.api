using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.DeleteBase;
using Core.Application.Models.Common;
using Core.Domain.Entities;
using Unit = MediatR.Unit;

namespace Core.Application.Features.Categories.Commands.DeleteCategory
{
    public record DeleteCategoryCommand : BaseDto, IRequest<Unit>
    {
    }

    public class DeleteCategoryCommandHandler :
        DeleteBaseCommandHandler<DeleteBaseCommandValidator<DeleteCategoryCommand>, DeleteCategoryCommand, Category>
    {
        public DeleteCategoryCommandHandler(ISupermarketDbContext pContext, IMapper pMapper, IMediator pMediator) : base(pContext, pMapper, pMediator) { }
    }
}