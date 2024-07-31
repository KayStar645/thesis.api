using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.DeleteBase;
using Core.Application.Models.Common;
using Core.Domain.Entities;

namespace Core.Application.Features.Products.Commands.DeleteProdct
{
    public record DeleteProductCommand : BaseDto, IRequest<Unit>
    {
    }

    public class DeleteProductCommandHandler :
        DeleteBaseCommandHandler<DeleteBaseCommandValidator<DeleteProductCommand>, DeleteProductCommand, Product>
    {
        public DeleteProductCommandHandler(ISupermarketDbContext pContext, IMapper pMapper, IMediator pMediator) : base(pContext, pMapper, pMediator) { }
    }
}