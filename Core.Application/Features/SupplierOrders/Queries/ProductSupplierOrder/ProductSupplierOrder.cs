using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Microsoft.AspNetCore.Http;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.SupplierOrders.Queries.ProductSupplierOrder
{
    public record ProductSupplierOrderCommand : DetailBaseCommand, IRequest<Result<List<ProductSupplierOrderDto>>>
    {
    }

    public class ProductSupplierOrderCommandHandler 
        : IRequestHandler<ProductSupplierOrderCommand, Result<List<ProductSupplierOrderDto>>>
    {
        private readonly ISupermarketDbContext _context;
        private readonly IMapper _mapper;
        protected readonly IMediator _mediator;

        public ProductSupplierOrderCommandHandler(
            ISupermarketDbContext pContext,
            IMapper pMapper,
            IMediator mediator
            )
        {
            _context = pContext;
            _mapper = pMapper;
            _mediator = mediator;
        }

        public async Task<Result<List<ProductSupplierOrderDto>>> Handle(ProductSupplierOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new ProductSupplierOrderValidator(_context);
                var validationResult = await validator.ValidateAsync(request);

                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(x => x.ErrorMessage).ToList();
                    return PaginatedResult<List<ProductSupplierOrderDto>>.Failure(StatusCodes.Status400BadRequest, errorMessages);
                }
                var products = await _context.DetailSupplierOrders
                        .Where(x => x.SupplierOrderId == request.Id)
                        .Include(x => x.Product)
                        .Include(x => x.Product.Category)
                        .Select(x => new ProductSupplierOrderDto
                        {
                            Id = x.Product.Id,
                            InternalCode = x.Product.InternalCode,
                            Name = x.Product.Name,
                            Images = _mapper.Map<List<string>>(x.Product.Images),
                            Price = x.Product.Price,
                            Quantity = x.Product.Quantity,
                            Describes = x.Product.Describes,
                            Feature = x.Product.Feature,
                            Specifications = x.Product.Specifications,
                            Status = x.Product.Status,
                            Category = new CategoryDto
                            {
                                Id = x.Product.Category.Id,
                                Name = x.Product.Category.Name
                            },
                            OrderQuantity = x.Quantity,
                            ImportQuantity = 0
                        })
                        .ToListAsync();
                foreach (var product in products)
                {
                    product.ImportQuantity = await _context.DetailSupplierOrders
                        .Include(x => x.SupplierOrder)
                        .Where(x => x.SupplierOrder.ParentId == request.Id &&
                                    x.SupplierOrder.Status == SupplierOrderStatus.Completed &&
                                    x.SupplierOrder.Type == SupplierOrderType.Receive &&
                                    x.ProductId == product.Id)
                        .SumAsync(x => x.Quantity);
                }

                var mapResults = _mapper.Map<List<ProductSupplierOrderDto>>(products);

                return Result<List<ProductSupplierOrderDto>>.Success(mapResults, StatusCodes.Status200OK);

            }
            catch (Exception ex)
            {
                return Result<List<ProductSupplierOrderDto>>.Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
