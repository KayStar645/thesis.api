using Core.Application.Common.Constants;
using Core.Application.Common.Interfaces;
using Core.Application.Extensions;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Core.Application.Features.Orders.Queries.DetailCart
{
    public record DetailCartCommand : IRequest<Result<CartDto>>
    {
        
    }

    public class DetailCartCommandHandler : IRequestHandler<DetailCartCommand, Result<CartDto>>
    {
        protected readonly ISupermarketDbContext _context;
        protected readonly IMapper _mapper;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAmazonS3Service _amazonS3;

        public DetailCartCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator, ICurrentUserService pCurrentUserService,
            IAmazonS3Service amazonS3)
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = pMediator;
            _currentUserService = pCurrentUserService;
            _amazonS3 = amazonS3;
        }

        public async Task<Result<CartDto>> Handle(DetailCartCommand request, CancellationToken cancellationToken)
        {
            if(_currentUserService.Type != CLAIMS_VALUES.TYPE_USER)
            {
                return Result<CartDto>.Failure("Vui lòng đăng ký tài khoản người dùng!",
                    StatusCodes.Status403Forbidden);
            }

            var query = _context.Set<Order>()
                .FilterDeleted()
                .Where(x => x.CustomerId == _currentUserService.CustomerId &&
                            x.Status == Order.OrderStatus.Cart);

            query = ApplyQuery(request, query);

            var findEntity = await query.SingleOrDefaultAsync();

            var dto = _mapper.Map<CartDto>(findEntity);

            var resultDto = await HandlerDtoAfterQuery(dto);

            return Result<CartDto>.Success(resultDto, StatusCodes.Status200OK);
        }

        private IQueryable<Order> ApplyQuery(DetailCartCommand request, IQueryable<Order> query)
        {
            query = query
                    .Include(x => x.Payment)
                    .Include(x => x.Customer)
                    .Include(x => x.Delivery)
                    .Include(x => x.StaffApproved);
            return query;
        }

        private async Task<CartDto> HandlerDtoAfterQuery(CartDto dto)
        {
            if(dto != null)
            {
                var details = await _context.DetailOrders
                    .Include(x => x.Product)
                    .Where(x => x.OrderId == dto.Id).ToListAsync();
                dto.Details = _mapper.Map<List<DetailCartDto>>(details); 
            }
            return dto;
        }
    }
}
