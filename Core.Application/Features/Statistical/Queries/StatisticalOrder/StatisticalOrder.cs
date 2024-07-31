using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using Sieve.Services;
using static Core.Domain.Entities.Order;

namespace Core.Application.Features.Statistical.Queries.StatisticalOrder
{
    public record StatisticalOrderCommand : IRequest<Result<List<StatisticalOrderDto>>>
    {
        public bool IsYear { get; set; }

        public int? Year { get; set; }

        public int? Month { get; set; }
    }

    public class StatisticalOrderCommandHandler : IRequestHandler<StatisticalOrderCommand, Result<List<StatisticalOrderDto>>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        protected readonly ISieveProcessor _sieveProcessor;

        public StatisticalOrderCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ISieveProcessor pSieveProcessor
            )
        {
            _context = pContext;
            _mediator = mediator;
            _sieveProcessor = pSieveProcessor;
        }

        public async Task<Result<List<StatisticalOrderDto>>> Handle(StatisticalOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                List<StatisticalOrderDto> result = new List<StatisticalOrderDto>();
                if (request.IsYear)
                {
                    for (int month = 1; month <= 12; month++)
                    {
                        StatisticalOrderDto item = new StatisticalOrderDto();
                        item.Time = month;
                        item.OrderCount = await _context.Orders
                                .CountAsync(x => x.IsDeleted == false &&
                                    x.Status == OrderStatus.Received &&
                                    x.UpdatedAt.Value.Year == request.Year &&
                                    x.UpdatedAt.Value.Month == month);
                        result.Add(item);
                    }
                }
                else
                {
                    int numberDay = DateTime.DaysInMonth((int)request.Year, (int)request.Month);
                    for (int day = 1; day <= numberDay; day++)
                    {
                        StatisticalOrderDto item = new StatisticalOrderDto();
                        item.Time = day;
                        item.OrderCount = await _context.Orders
                                .CountAsync(x => x.IsDeleted == false &&
                                    x.Status == OrderStatus.Received &&
                                    x.UpdatedAt.Value.Year == request.Year &&
                                    x.UpdatedAt.Value.Month == request.Month &&
                                    x.UpdatedAt.Value.Day == day);
                        result.Add(item);
                    }
                }


                return Result<List<StatisticalOrderDto>>.Success(result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<List<StatisticalOrderDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
