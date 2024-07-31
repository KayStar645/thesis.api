using Core.Application.Common.Interfaces;
using Core.Application.Features.Statistical.Queries.StatisticalInventory;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using Sieve.Services;
using static Core.Domain.Entities.Order;

namespace Core.Application.Features.Statistical.Queries.StatisticalSelling
{
    public record StatisticalSellingCommand : IRequest<Result<List<StatisticalSellingDto>>>
    {
        public bool IsYear { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }
    }

    public class StatisticalSellingCommandHandler : IRequestHandler<StatisticalSellingCommand, Result<List<StatisticalSellingDto>>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        protected readonly ISieveProcessor _sieveProcessor;

        public StatisticalSellingCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ISieveProcessor pSieveProcessor
            )
        {
            _context = pContext;
            _mediator = mediator;
            _sieveProcessor = pSieveProcessor;
        }

        public async Task<Result<List<StatisticalSellingDto>>> Handle(StatisticalSellingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<StatisticalSellingDto> result = new List<StatisticalSellingDto>();
                if (request.IsYear)
                {
                    var temp = _context.DetailOrders
                            .Include(x => x.Order)
                            .Where(x => x.Order.IsDeleted == false &&
                                        x.Order.Status == OrderStatus.Received &&
                                        x.Order.UpdatedAt.HasValue &&
                                        x.Order.UpdatedAt.Value.Year == request.Year)
                            .GroupBy(x => x.ProductId)
                            .Select(g => new
                            {
                                Number = g.Key.ToString(),
                                ParentId = g.Select(x => x.Product.CategoryId).FirstOrDefault(),
                                Name = g.Select(x => x.Product.Name).First(),
                                Value = g.Sum(x => x.Quantity)
                            })
                            .OrderByDescending(x => x.Value)
                            .ToList();

                    result.AddRange(temp.Select(group => new StatisticalSellingDto
                    {
                        Number = group.Number,
                        Name = group.Name,
                        Value = group.Value
                    }));


                    foreach (var item in temp)
                    {
                        var category = await _context.Categories.FindAsync(item.ParentId);
                        while (category != null)
                        {
                            var index = result.FindLastIndex(x => x.Number == "1100" + category.Id);
                            if (index == -1)
                            {
                                StatisticalSellingDto parent = new StatisticalSellingDto();
                                parent.Number = "1100" + category.Id.ToString();
                                parent.Name = category.Name;
                                parent.Value = item.Value;
                                result.Add(parent);
                            }
                            else
                            {
                                var parent = result[index];
                                parent.Value += item.Value;
                                result[index] = parent;
                            }
                            category = await _context.Categories.FindAsync(category.ParentId);
                        }
                    }    
                }
                else
                {
                    var temp = _context.DetailOrders
                            .Include(x => x.Order)
                            .Where(x => x.Order.IsDeleted == false &&
                                        x.Order.Status == OrderStatus.Received &&
                                        x.Order.UpdatedAt.HasValue &&
                                        x.Order.UpdatedAt.Value.Year == request.Year &&
                                        x.Order.UpdatedAt.Value.Month == request.Month)
                            .GroupBy(x => x.ProductId)
                            .Select(g => new
                            {
                                Number = g.Key.ToString(),
                                ParentId = g.Select(x => x.Product.CategoryId).FirstOrDefault(),
                                Name = g.Select(x => x.Product.Name).First(),
                                Value = g.Sum(x => x.Quantity)
                            })
                            .OrderByDescending(x => x.Value)
                            .ToList();

                    result.AddRange(temp.Select(group => new StatisticalSellingDto
                    {
                        Number = group.Number,
                        Name = group.Name,
                        Value = group.Value
                    }));

                    foreach (var item in temp)
                    {
                        var category = await _context.Categories.FindAsync(item.ParentId);
                        while (category != null)
                        {
                            var index = result.FindLastIndex(x => x.Number == "1100" + category.Id);
                            if (index == -1)
                            {
                                StatisticalSellingDto parent = new StatisticalSellingDto();
                                parent.Number = "1100" + category.Id.ToString();
                                parent.Name = category.Name;
                                parent.Value = item.Value;
                                result.Add(parent);
                            }
                            else
                            {
                                var parent = result[index];
                                parent.Value += item.Value;
                                result[index] = parent;
                            }
                            category = await _context.Categories.FindAsync(category.ParentId);
                        }
                    }
                }
                result = result.OrderBy(x => x.Value).TakeLast(10).ToList();
                return Result<List<StatisticalSellingDto>>.Success(result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<List<StatisticalSellingDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
