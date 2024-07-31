using Core.Application.Common.Interfaces;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.SupplierOrders.Commands.ChangeStatusSupplierOrder
{
    public record ChangeStatusSupplierOrderCommand : IRequest<Result<bool>>
    {
        public int? SupplierOrderId { get; set; }

        public SupplierOrderStatus? Status { get; set; }
    }

    public class ChangeStatusSupplierOrderCommandHandler : 
        IRequestHandler<ChangeStatusSupplierOrderCommand, Result<bool>>
    {
        private readonly ISupermarketDbContext _context;
        protected readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public ChangeStatusSupplierOrderCommandHandler(
            ISupermarketDbContext pContext,
            IMediator mediator,
            ICurrentUserService currentUserService)
        {
            _context = pContext;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<Result<bool>> Handle(ChangeStatusSupplierOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ChangeStatusSupplierOrderValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (validationResult.IsValid == false)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return Result<bool>.Failure(errorMessages, StatusCodes.Status400BadRequest);
                }

                var order = await _context.SupplierOrders.FindAsync(request.SupplierOrderId);

                order.Status = request.Status;
                order.ApproveStaffId = _currentUserService.StaffId;

                _context.SupplierOrders.Update(order);
                await _context.SaveChangesAsync(cancellationToken);

                return Result<bool>.Success(true, StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
