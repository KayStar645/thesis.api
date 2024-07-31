using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using Sieve.Services;

namespace Core.Application.Features.Report.Queries.ReportProfitCategory
{
    public record ReportProfitCategoryCommand : IRequest<Result<ReportProfitCategoryDto>>
    {
        public int? pMonth { get; set; }

        public int? pYear { get; set; }
    }

    public class ReportProfitCategoryCommandHandler : IRequestHandler<ReportProfitCategoryCommand, Result<ReportProfitCategoryDto>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMediator _mediator;
        private readonly ISieveProcessor _sieveProcessor;
        private readonly ICLHUIService _clhuService;

        public ReportProfitCategoryCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ISieveProcessor pSieveProcessor,
            ICLHUIService clhuService
            )
        {
            _context = pContext;
            _mediator = mediator;
            _sieveProcessor = pSieveProcessor;
            _clhuService = clhuService;
        }

        public async Task<Result<ReportProfitCategoryDto>> Handle(ReportProfitCategoryCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ReportProfitCategoryDto result = new ReportProfitCategoryDto();

                result.CompanyName = "CÔNG TY TNHH 3V";
                result.Name = "BÁO CÁO LỢI NHUẬN THEO DANH MỤC";
                result.Time = $"Thời gian: {request.pMonth}/{request.pYear}";
                result.Address = "140 Lê Trọng Tấn, Tây Thạnh, Tân Phú";

                return Result<ReportProfitCategoryDto>.Success(result, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<ReportProfitCategoryDto>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
