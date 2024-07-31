using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Core.Application.Services;
using Microsoft.AspNetCore.Http;
using Sieve.Services;
using static Core.Domain.Entities.Order;

namespace Core.Application.Features.Report.Queries.ReportProfit
{
    public record ReportProfitCommand : IRequest<Result<ReportProfitDto>>
    {
        public int pMinUtil { get; set; }

        public int? pMonth { get; set; }

        public int? pYear { get; set; }
    }

    public class ReportProfitCommandHandler : IRequestHandler<ReportProfitCommand, Result<ReportProfitDto>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMediator _mediator;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly ICLHUIService _clhuService;
        protected readonly ICurrentUserService _currentUserService;

        public ReportProfitCommandHandler(
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

        public async Task<Result<ReportProfitDto>> Handle(ReportProfitCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReportProfitDto result = new ReportProfitDto();
                List<CLHUIs> clhuis = await _clhuService.RunAlgorithm(request.pMinUtil, request.pMonth, request.pYear);

                result.Rows = clhuis.Select(x => new ReportProfitRow
                {
                    Id = x.Id,
                    GroupProfits = x.Profit,
                    Items = x.Items.Select(y => new ReportProfitItem
                    {
                        Name = y.Name,
                        SellNumber = 0,
                        Revenue = 0,
                        Expense = 0,
                        Profit = y.IsCategory ? -2 : -1,
                        IsCategory = y.IsCategory
                    }).ToList(),
                }).ToList();


                var query = _context.DetailOrders
                .Where(x => x.Order.Status == OrderStatus.Transport ||
                            x.Order.Status == OrderStatus.Received)
                .AsQueryable();
                if (request.pMonth != null)
                {
                    query = query.Where(x => x.Order.UpdatedAt.HasValue &&
                                            x.Order.UpdatedAt.Value.Year == request.pYear &&
                                            x.Order.UpdatedAt.Value.Month == request.pMonth)
                        .AsQueryable();
                }
                else if (request.pYear != null)
                {
                    query = query.Where(x => x.Order.UpdatedAt.HasValue &&
                                            x.Order.UpdatedAt.Value.Year == request.pYear)
                        .AsQueryable();
                }
                result.Rows = result.Rows.OrderByDescending(item => item.GroupProfits)
                    .Take(20)
                    .ToList();
                for (int i = 0; i < result.Rows.Count; i++)
                {
                    for(int j = 0; j < result.Rows[i].Items.Count; j++)
                    {
                        string name = result.Rows[i].Items[j].Name;
                        if (result.Rows[i].Items[j].Profit == -1)
                        {
                            var foundItem = result.Rows.Select(row => row.Items
                            .FirstOrDefault(item => item.Name == name && item.Profit != -2))
                                .FirstOrDefault();
                            if (foundItem != null)
                            {
                                result.Rows[i].Items[j].SellNumber = foundItem.SellNumber;
                                result.Rows[i].Items[j].Revenue = foundItem.Revenue;
                                result.Rows[i].Items[j].Profit = foundItem.Profit;
                                result.Rows[i].Items[j].Expense = foundItem.Revenue - foundItem.Profit;
                                continue;
                            }
                            var newQuery = query.Where(x => x.Product.Name == name);
                            result.Rows[i].Items[j].SellNumber = await newQuery.SumAsync(x => x.Quantity);
                            result.Rows[i].Items[j].Revenue = await newQuery.SumAsync(x => x.Price);
                            result.Rows[i].Items[j].Profit = await newQuery.SumAsync(x => x.Profit);
                            result.Rows[i].Items[j].Expense = result.Rows[i].Items[j].Revenue - result.Rows[i].Items[j].Profit;
                        }
                        else
                        {
                            var foundItem = result.Rows.Select(row => row.Items
                            .FirstOrDefault(item => item.Name == name && item.Profit != -2))
                                .FirstOrDefault();
                            if(foundItem != null)
                            {
                                result.Rows[i].Items[j].SellNumber = foundItem.SellNumber;
                                result.Rows[i].Items[j].Revenue = foundItem.Revenue;
                                result.Rows[i].Items[j].Profit = foundItem.Profit;
                                result.Rows[i].Items[j].Expense = foundItem.Revenue - foundItem.Profit;
                                continue;
                            }
                            List<string> names = new List<string>();
                            names = await GetProducts(names, result.Rows[i].Items[j].Name);

                            var newQuery = query.Where(x => names.Contains(x.Product.Name));
                            result.Rows[i].Items[j].SellNumber = await newQuery.SumAsync(x => x.Quantity);
                            result.Rows[i].Items[j].Revenue = await newQuery.SumAsync(x => x.Price);
                            result.Rows[i].Items[j].Profit = await newQuery.SumAsync(x => x.Profit);
                            result.Rows[i].Items[j].Expense = result.Rows[i].Items[j].Revenue - result.Rows[i].Items[j].Profit;

                        }
                    }
                }
                result.CompanyName = "CÔNG TY TNHH 3V";
                result.Name = "BÁO CÁO LỢI NHUẬN";
                result.Time = $"Thời gian: {request.pMonth}/{request.pYear}";
                result.Address = "140 Lê Trọng Tấn, Tây Thạnh, Tân Phú";
                result.Revenue = result.Rows.Sum(x => x.Items.Sum(y => y.Revenue));
                result.Expense = result.Rows.Sum(x => x.Items.Sum(y => y.Expense));
                result.Profit = result.Rows.Sum(x => (double)x.Items.Sum(y => y.Revenue - y.Expense));
                string createBy = "";
                if(_currentUserService.Type == CLAIMS_VALUES.TYPE_ADMIN)
                {
                    createBy = (await _context.Users.FindAsync(_currentUserService.UserId)).UserName;
                }
                else if(_currentUserService.Type == CLAIMS_VALUES.TYPE_SUPER_ADMIN)
                {
                    createBy = (await _context.Staffs.FindAsync(_currentUserService.StaffId)).Name;
                }
                result.CreateBy = createBy;

                return Result<ReportProfitDto>.Success(result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<ReportProfitDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        private async Task<List<string>> GetProducts(List<string> result, string pCategoryName)
        {
            var category = await _context.Categories
                .Where(x => x.Name == pCategoryName)
                .FirstOrDefaultAsync();
            if (category != null)
            {
                var categories = await _context.Categories
                .Where(x => x.ParentId == category.Id)
                .ToListAsync();
                foreach(var item in categories)
                {
                    result = await GetProducts(result, item.Name);
                }
            }
            var products = await _context.Products
                .Where(x => x.CategoryId == category.Id)
                .Select(x => x.Name)
                .ToListAsync();
            return result.Union(products).ToList();
        }
    }
}
