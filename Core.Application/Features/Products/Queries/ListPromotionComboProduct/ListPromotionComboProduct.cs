using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetListBase;
using Core.Application.Features.Base.Queries.ListBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Sieve.Models;
using Sieve.Services;
using static Core.Domain.Entities.Promotion;

namespace Core.Application.Features.Products.Queries.ListPromotionComboProduct
{
    public record ListPromotionComboProductCommand : ListBaseCommand, IRequest<PaginatedResult<List<PromotionComboProductDto>>>
    {
    }

    public class ListPromotionComboProductCommandHandler : IRequestHandler<ListPromotionComboProductCommand, PaginatedResult<List<PromotionComboProductDto>>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMapper _mapper;
        protected readonly IMediator _mediator;
        protected readonly ISieveProcessor _sieveProcessor;

        public ListPromotionComboProductCommandHandler(
            ISupermarketDbContext pContext,
            IMapper pMapper,
            IMediator mediator,
            ISieveProcessor pSieveProcessor
            )
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = mediator;
            _sieveProcessor = pSieveProcessor;
        }

        public async Task<PaginatedResult<List<PromotionComboProductDto>>> Handle(ListPromotionComboProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ListPromotionComboProductWithPermissionValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return PaginatedResult<List<PromotionComboProductDto>>.Failure(StatusCodes.Status400BadRequest, errorMessages);
                }
                var query = _context.PromotionProductRequirements
                    .Include(x => x.Promotion)
                    .Include(x => x.Product)
                    .Where(x => x.Promotion.Start <= DateTime.Now &&
                                    DateTime.Now <= x.Promotion.End &&
                                    x.Promotion.Limit >= 1 &&
                                    x.Promotion.Status == PromotionStatus.Approve)
                    .GroupBy(x => x.Group)
                    .AsQueryable();

                request.PageSize = 1000;
                var sieve = _mapper.Map<SieveModel>(request);

                int totalCount = await PaginatedResultBase.CountApplySieveAsync(_sieveProcessor, sieve, query);

                var temp = await query.ToListAsync();

                var results = temp
                    .Select(g => new PromotionComboProductDto
                    {
                        Id = g.Key,
                        Products = g.Select(p => _mapper.Map<ProductDto>(p.Product)).ToList(),
                        Promotion = _mapper.Map<PromotionDto>(g.Select(p => p.Promotion).FirstOrDefault()),
                    })
                    .ToList();
                List<PromotionComboProductDto> results2 = new List<PromotionComboProductDto>();
                for(int i = 0; i < results.Count; i++)
                {
                    int number = results[i].Products.Count();
                    if(number == 1)
                    {
                        continue;
                    }
                    decimal? price = 0;
                    decimal? priceDiscout = 0;
                    for(int j = 0; j < results[i].Products.Count(); j++)
                    {
                        price += results[i].Products[j].Price;
                        if (results[i].Promotion.Type == PromotionType.Percent)
                        {
                            priceDiscout = results[i].Products[j].Price * (results[i].Promotion.Percent * 0.01m) > results[i].Promotion.DiscountMax ?
                                            results[i].Promotion.DiscountMax : results[i].Products[j].Price * (results[i].Promotion.Percent * 0.01m);
                            results[i].Products[j].NewPrice = results[i].Products[j].Price - priceDiscout / number;
                        }
                        else if (results[i].Promotion.Type == PromotionType.Discount)
                        {
                            priceDiscout = (results[i].Promotion.Discount / number) > results[i].Products[j].Price * (results[i].Promotion.PercentMax * 0.01m) ?
                                            results[i].Products[j].Price * (results[i].Promotion.PercentMax * 0.01m) : (results[i].Promotion.Discount / number);
                            results[i].Products[j].NewPrice = results[i].Products[j].Price - priceDiscout / number;
                        }
                    }
                    results[i].Price = price;
                    results[i].ReducedPrice = priceDiscout;
                    results[i].NewPrice = price - priceDiscout;
                    results2.Add(results[i]);
                }    

                return PaginatedResult<List<PromotionComboProductDto>>.Success(results2, totalCount, request.Page, request.PageSize);
            }
            catch (Exception ex)
            {
                return PaginatedResult<List<PromotionComboProductDto>>.Failure(StatusCodes.Status500InternalServerError,
                    new List<string> { ex.Message });
            }
        }
    }
}
