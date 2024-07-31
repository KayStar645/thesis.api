using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using Sieve.Services;
using static Core.Domain.Entities.Order;
using static Core.Domain.Entities.Product;

namespace Core.Application.Features.Statistical.Queries.StatisticalOverview
{
    public record StatisticalOverviewCommand : IRequest<Result<StatisticalOverviewDto>>
    {
    }

    public class StatisticalOverviewCommandHandler : IRequestHandler<StatisticalOverviewCommand, Result<StatisticalOverviewDto>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        protected readonly ISieveProcessor _sieveProcessor;

        public StatisticalOverviewCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ISieveProcessor pSieveProcessor
            )
        {
            _context = pContext;
            _mediator = mediator;
            _sieveProcessor = pSieveProcessor;
        }

        public async Task<Result<StatisticalOverviewDto>> Handle(StatisticalOverviewCommand request, CancellationToken cancellationToken)
        {
            try
            {
                StatisticalOverviewDto result = new StatisticalOverviewDto();

                result.ItemTotal = await _context.Products
                    .CountAsync(x => x.IsDeleted == false && 
                                     x.Type == ProductType.Option);
                result.CustomerTotal = await _context.Customers
                    .CountAsync(x => x.IsDeleted == false);
                result.OrderTotal = await _context.Orders
                    .CountAsync(x => x.IsDeleted == false &&
                                     x.Status == OrderStatus.Received);
                result.OutStock = await _context.Products
                    .CountAsync(x => x.IsDeleted == false &&
                                     x.Type == ProductType.Option &&
                                     x.Quantity == 0);

                return Result<StatisticalOverviewDto>.Success(result, StatusCodes.Status200OK);
            }   
            catch (Exception ex)
            {
                return Result<StatisticalOverviewDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
