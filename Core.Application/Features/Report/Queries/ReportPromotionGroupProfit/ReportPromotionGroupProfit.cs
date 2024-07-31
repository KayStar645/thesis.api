using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using Sieve.Services;
using static Core.Domain.Entities.Order;

namespace Core.Application.Features.Report.Queries.ReportPromotionGroupProfit
{
    public record ReportPromotionGroupProfitCommand : IRequest<Result<ReportPromotionGroupProfitDto>>
    {
        public int? pMonth { get; set; }

        public int? pYear { get; set; }
    }

    public class ReportPromotionGroupProfitCommandHandler : IRequestHandler<ReportPromotionGroupProfitCommand, Result<ReportPromotionGroupProfitDto>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMediator _mediator;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly ICLHUIService _clhuService;
        protected readonly ICurrentUserService _currentUserService;

        public ReportPromotionGroupProfitCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ISieveProcessor pSieveProcessor,
            ICLHUIService clhuService,
            ICurrentUserService currentUserService
            )
        {
            _context = pContext;
            _mediator = mediator;
            _sieveProcessor = pSieveProcessor;
            _clhuService = clhuService;
            _currentUserService = currentUserService;
        }

        public async Task<Result<ReportPromotionGroupProfitDto>> Handle(ReportPromotionGroupProfitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReportPromotionGroupProfitDto result = new ReportPromotionGroupProfitDto();
                var query = _context.DetailOrders
                .Where(x => x.Order.Status == OrderStatus.Transport ||
                            x.Order.Status == OrderStatus.Received &&
                            x.GroupPromotion != null)
                .AsQueryable();
                if (request.pMonth != null)
                {
                    query = query.Where(x => x.Order.UpdatedAt.HasValue &&
                                            x.Order.UpdatedAt.Value.Year == request.pYear &&
                                            x.Order.UpdatedAt.Value.Month == request.pMonth);
                }
                else if (request.pYear != null)
                {
                    query = query.Where(x => x.Order.UpdatedAt.HasValue &&
                                            x.Order.UpdatedAt.Value.Year == request.pYear);
                }

                result.Rows = await query
                    .GroupBy(x => x.GroupPromotion)
                    .Select(x => new ReportPromotionGroupProfitRow
                    {
                        Id = x.Key,
                        Names = x.Select(x => x.Product.Name).Distinct().ToList(),
                        SellNumber = x.Select(x => x.OrderId).Distinct().Count(),
                        Revenue = x.Sum(x => x.Price),
                        Profit = x.Sum(x => (double)x.Profit),
                        Expense = x.Sum(x => x.Price - x.Profit),
                    })
                    .ToListAsync();

                result.CompanyName = "CÔNG TY TNHH 3V";
                result.Name = "BÁO CÁO LỢI NHUẬN THEO DANH MỤC";
                result.Time = $"Thời gian: {request.pMonth}/{request.pYear}";
                result.Address = "140 Lê Trọng Tấn, Tây Thạnh, Tân Phú";
                result.Revenue = result.Rows.Sum(x => x.Revenue);
                result.Expense = result.Rows.Sum(x => x.Expense);
                result.Profit = result.Rows.Sum(x => x.Profit);
                string createBy = "";
                if (_currentUserService.Type == CLAIMS_VALUES.TYPE_ADMIN)
                {
                    createBy = (await _context.Users.FindAsync(_currentUserService.UserId)).UserName;
                }
                else if (_currentUserService.Type == CLAIMS_VALUES.TYPE_SUPER_ADMIN)
                {
                    createBy = (await _context.Staffs.FindAsync(_currentUserService.StaffId)).Name;
                }
                result.CreateBy = createBy;

                return Result<ReportPromotionGroupProfitDto>.Success(result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<ReportPromotionGroupProfitDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
