using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using Sieve.Services;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.Statistical.Queries.StatisticalInventory
{
    public record StatisticalInventoryCommand : IRequest<Result<List<StatisticalInventoryDto>>>
    {
    }

    public class StatisticalInventoryCommandHandler : IRequestHandler<StatisticalInventoryCommand, Result<List<StatisticalInventoryDto>>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        protected readonly ISieveProcessor _sieveProcessor;

        public StatisticalInventoryCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ISieveProcessor pSieveProcessor
            )
        {
            _context = pContext;
            _mediator = mediator;
            _sieveProcessor = pSieveProcessor;
        }

        public async Task<Result<List<StatisticalInventoryDto>>> Handle(StatisticalInventoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<StatisticalInventoryDto> result = new List<StatisticalInventoryDto>();
                var products = await _context.Products
                    .Where(x => x.IsDeleted == false && x.Type == ProductType.Option)
                    .ToListAsync();
                foreach (var product in products)
                {
                    StatisticalInventoryDto item = new StatisticalInventoryDto();
                    item.Id = product.Id.ToString();
                    item.Parent = "1100" + product.CategoryId.ToString();
                    item.Name = product.Name;
                    item.Value = product.Quantity;
                    result.Add(item);

                    var category = await _context.Categories.FindAsync(product.CategoryId);
                    while(category != null)
                    {
                        var index = result.FindLastIndex(x => x.Id == "1100" + category.Id);
                        if(index == -1)
                        {
                            StatisticalInventoryDto parent = new StatisticalInventoryDto();
                            parent.Id = "1100" + category.Id.ToString();
                            parent.Parent = "1100" + category.ParentId.ToString() == "1100" ? "" : "1100" + category.ParentId.ToString();
                            parent.Name = category.Name;
                            parent.Value = product.Quantity;
                            result.Add(parent);
                        }
                        else
                        {
                            var parent = result[index];
                            parent.Value += product.Quantity;
                            result[index] = parent;
                        }
                        category = await _context.Categories.FindAsync(category.ParentId);
                    }
                }



                return Result<List<StatisticalInventoryDto>>.Success(result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<List<StatisticalInventoryDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
