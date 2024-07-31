using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Promotions.Commands.ApplyPromotionForProduct
{
    public record ApplyPromotionForProductCommand : IRequest<Result<bool>>
    {
        public int? PromotionId { get; set; }

        public List<int>? ProductsId { get; set; }

        public int? Group { get; set; }
    }

    public class ApplyPromotionForProductCommandHandler : IRequestHandler<ApplyPromotionForProductCommand, Result<bool>>
    {
        public readonly ISupermarketDbContext _context;
        public readonly IMapper _mapper;

        public ApplyPromotionForProductCommandHandler(ISupermarketDbContext pContext, IMapper pMapper)
        {
            _context = pContext;
            _mapper = pMapper;
        }

        public async Task<Result<bool>> Handle(ApplyPromotionForProductCommand request, CancellationToken cancellationToken)
        {
            var validator = new ApplyPromotionForProductValidator(_context);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }

            if (request.Group != -1)
            {
                // Cập nhật lại danh sách khuyến mãi cũ                                                                                                                  
                var productsId = await _context.PromotionProductRequirements
                                    .Where(x => x.Group == request.Group)
                                    .Select(x => x.ProductId)
                                    .ToListAsync();

                var productIdDelete = productsId.Cast<int>().Except(request.ProductsId).ToList();
                var productIdCreate = request.ProductsId.Except(productsId.Cast<int>()).ToList();

                foreach (var productId in productIdDelete)
                {
                    var detail = await _context.PromotionProductRequirements
                        .Where(x => x.PromotionId == request.PromotionId && x.ProductId == productId &&
                                    x.Group == request.Group)
                        .FirstOrDefaultAsync();
                    _context.PromotionProductRequirements.Remove(detail);
                }
                await _context.SaveChangesAsync(cancellationToken);

                foreach (var productId in productIdCreate)
                {
                    var detail = new PromotionProductRequirement
                    {
                        PromotionId = request.PromotionId,
                        ProductId = productId,
                        Group = request.Group,
                    };
                    await _context.PromotionProductRequirements.AddAsync(detail);
                }
                await _context.SaveChangesAsync(cancellationToken);
            }
            else
            {
                // Tạo mới 1 group sản phẩm để áp dụng khuyến mãi
                var maxGroup = await _context.PromotionProductRequirements.MaxAsync(x => x.Group);
                int? newGroup = maxGroup == null ? 0 : maxGroup + 1;
                foreach (var productId in request.ProductsId)
                {
                    var detail = new PromotionProductRequirement
                    {
                        PromotionId = request.PromotionId,
                        ProductId = productId,
                        Group = newGroup
                    };
                    await _context.PromotionProductRequirements.AddAsync(detail);
                }
                await _context.SaveChangesAsync(cancellationToken);
            }

            return Result<bool>.Success(true, StatusCodes.Status200OK);
        }
    }
}