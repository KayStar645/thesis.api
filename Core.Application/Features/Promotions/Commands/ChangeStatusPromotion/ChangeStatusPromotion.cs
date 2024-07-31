using Core.Application.Common.Interfaces;
using Core.Application.Models;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.Promotion;

namespace Core.Application.Features.Promotions.Commands.ChangeStatusPromotion
{
    public record ChangeStatusPromotionCommand : IRequest<Result<PromotionDto>>
    {
        public int? PromotionId { get; set; }

        public PromotionStatus? Status { get; set; }
    }

    public class ChangeStatusPromotionCommandHandler : IRequestHandler<ChangeStatusPromotionCommand, Result<PromotionDto>>
    {
        public readonly ISupermarketDbContext _context;
        public readonly IMapper _mapper;

        public ChangeStatusPromotionCommandHandler(ISupermarketDbContext pContext, IMapper pMapper)
        {
            _context = pContext;
            _mapper = pMapper;
        }

        public async Task<Result<PromotionDto>> Handle(ChangeStatusPromotionCommand request, CancellationToken cancellationToken)
        {
            var validator = new ChangeStatusPromotionValidator(_context);
            var validationResult = await validator.ValidateAsync(request);

            if (validationResult.IsValid == false)
            {
                var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                return Result<PromotionDto>.Failure(errorMessages, StatusCodes.Status400BadRequest);
            }

            var findEntity = await _context.Promotions.FindAsync(request.PromotionId);

            bool flag1 = findEntity.Status == PromotionStatus.Draft &&
                (request.Status == PromotionStatus.Approve || request.Status == PromotionStatus.Cancel);
            bool flag2 = findEntity.Status == PromotionStatus.Approve &&
                (request.Status == PromotionStatus.Draft || request.Status == PromotionStatus.Cancel);

            if (!flag1 && !flag2)
            {
                return Result<PromotionDto>.Failure("Trạng thái không hợp lệ!", StatusCodes.Status400BadRequest);
            }

            findEntity.Status = request.Status;

            var newEntity = _context.Promotions.Update(findEntity);
            await _context.SaveChangesAsync(cancellationToken);

            var dto = _mapper.Map<PromotionDto>(newEntity.Entity);

            return Result<PromotionDto>.Success(dto, StatusCodes.Status200OK);
        }
    }
}
