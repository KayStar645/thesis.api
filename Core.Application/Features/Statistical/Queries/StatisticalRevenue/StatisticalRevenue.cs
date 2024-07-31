using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using Sieve.Services;
using static Core.Domain.Entities.Order;

namespace Core.Application.Features.Statistical.Queries.StatisticalRevenue
{
    public record StatisticalRevenueCommand : IRequest<Result<List<StatisticalRevenueDto>>>
    {
        public bool IsYear { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }
    }

    public class StatisticalRevenueCommandHandler : IRequestHandler<StatisticalRevenueCommand, Result<List<StatisticalRevenueDto>>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        protected readonly ISieveProcessor _sieveProcessor;

        public StatisticalRevenueCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ISieveProcessor pSieveProcessor
            )
        {
            _context = pContext;
            _mediator = mediator;
            _sieveProcessor = pSieveProcessor;
        }

        public async Task<Result<List<StatisticalRevenueDto>>> Handle(StatisticalRevenueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<StatisticalRevenueDto> result = new List<StatisticalRevenueDto>();
                if (request.IsYear)
                {
                    for (int month = 1; month <= 12; month++)
                    {
                        StatisticalRevenueDto item = new StatisticalRevenueDto();
                        item.Time = month;
                        item.Revenue = await _context.Orders
                                .Where(x => x.IsDeleted == false &&
                                    x.Status == OrderStatus.Received &&
                                    x.UpdatedAt.Value.Year == request.Year &&
                                    x.UpdatedAt.Value.Month == month)
                                .SumAsync(x => x.TotalAmount);
                        result.Add(item);
                    }
                }
                else
                {
                    int numberDay = DateTime.DaysInMonth((int)request.Year, (int)request.Month);
                    for (int day = 1; day <= numberDay; day++)
                    {
                        StatisticalRevenueDto item = new StatisticalRevenueDto();
                        item.Time = day;
                        item.Revenue = await _context.Orders
                                .Where(x => x.IsDeleted == false &&
                                    x.Status == OrderStatus.Received &&
                                    x.UpdatedAt.Value.Year == request.Year &&
                                    x.UpdatedAt.Value.Month == request.Month &&
                                    x.UpdatedAt.Value.Day == day)
                                .SumAsync(x => x.TotalAmount);
                        result.Add(item);
                    }
                }

                return Result<List<StatisticalRevenueDto>>.Success(result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<List<StatisticalRevenueDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
