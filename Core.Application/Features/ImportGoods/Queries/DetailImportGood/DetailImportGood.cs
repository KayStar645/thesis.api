using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Queries.GetRequestBase;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;
using static Core.Domain.Entities.SupplierOrder;

namespace Core.Application.Features.ImportGoods.Queries.DetailImportGood
{
    public record DetailImportGoodCommand : DetailBaseCommand, IRequest<Result<ImportGoodDto>>
    {
    }

    public class DetailImportGoodCommandHandler :
        DetailBaseCommandHandler<DetailImportGoodValidator, DetailImportGoodCommand, ImportGoodDto, SupplierOrder>
    {
        public DetailImportGoodCommandHandler(ISupermarketDbContext pContext,
            IMapper pMapper, IMediator pMediator,
            ICurrentUserService pCurrentUserService)
            : base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }

        protected override IQueryable<SupplierOrder> ApplyQuery(DetailImportGoodCommand request, IQueryable<SupplierOrder> query)
        {
            query = query.Include(x => x.Distributor)
                         .Include(x => x.Parent)
                         .Include(x => x.ApproveStaff);

            return query;
        }

        protected override async Task<ImportGoodDto> HandlerDtoAfterQuery(ImportGoodDto dto)
        {
            var details = await _context.DetailSupplierOrders
                .Include(x => x.Product)
                .Where(x => x.SupplierOrderId == dto.Id)
                .Select(x => new ProductImportGoodDto
                {
                    Id = x.Product.Id,
                    InternalCode = x.Product.InternalCode,
                    Name = x.Product.Name,
                    Images = _mapper.Map<List<string>>(x.Product.Images),
                    Price = x.Product.Price,
                    Quantity = x.Quantity,
                    Describes = x.Product.Describes,
                    Feature = x.Product.Feature,
                    Specifications = x.Product.Specifications,
                    Status = x.Product.Status,
                    Category = new CategoryDto
                    {
                        Id = x.Product.Category.Id,
                        Name = x.Product.Category.Name
                    },
                    OrderQuantity = 0,
                    ImportQuantity = 0
                })
                .ToListAsync();
                foreach (var product in details)
                {
                    product.ImportQuantity = await _context.DetailSupplierOrders
                        .Include(x => x.SupplierOrder)
                        .Where(x => x.SupplierOrder.ParentId == dto.ParentId &&
                                    x.SupplierOrder.Status == SupplierOrderStatus.Completed &&
                                    x.SupplierOrder.Type == SupplierOrderType.Receive &&
                                    x.ProductId == product.Id)
                        .SumAsync(x => x.Quantity);

                    product.OrderQuantity = await _context.DetailSupplierOrders
                        .Include(x => x.SupplierOrder)
                        .Where(x => x.SupplierOrder.Id == dto.ParentId &&
                                    (x.SupplierOrder.Status == SupplierOrderStatus.Order ||
                                    x.SupplierOrder.Status == SupplierOrderStatus.PartialReceipt) &&
                                    x.SupplierOrder.Type == SupplierOrderType.Order &&
                                    x.ProductId == product.Id)
                        .SumAsync(x => x.Quantity);
            }

            dto.Details = _mapper.Map<List<ProductImportGoodDto>>(details);

            return dto;
        }
    }
}
