using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.Products.Commands.ChangeStatusProduct
{
    public class ChangeStatusProductCommand : IRequest<Result<bool>>
    {
        public int? ProductId { get; set; }

        public ProductStatus? Status { get; set; }
    }

    public class ChangeStatusProductCommandHandler : IRequestHandler<ChangeStatusProductCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public ChangeStatusProductCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(ChangeStatusProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ChangeStatusProductValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                var product = await _context.Products.FindAsync(request.ProductId);

                product.Status = request.Status;

                _context.Products.Update(product);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(true, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
