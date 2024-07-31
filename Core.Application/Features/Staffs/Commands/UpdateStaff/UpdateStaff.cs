using Core.Application.Common.Interfaces;
using Core.Application.Features.Base.Commands.UpdateBase;
using Core.Application.Features.Base.Records;
using Core.Application.Features.Staffs.Commands.BaseStaff;
using Core.Application.Models;
using Core.Application.Responses;
using Core.Domain.Entities;

namespace Core.Application.Features.Staffs.Commands.UpdateStaff
{
    public record UpdateStaffCommand : UpdateBaseCommand, IBaseStaff, IRequest<Result<StaffDto>>
    {
        public string? InternalCode { get; set; }

        public string? Name { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? Avatar { get; set; }

        public string? IdCard { get; set; }

        public CardImage? IdCardImage { get; set; }

        public int? PositionId { get; set; }
    }

    public class UpdateStaffCommandHandler : 
        UpdateBaseCommandHandler<UpdateStaffValidator, UpdateStaffCommand, StaffDto, Staff>
    {
        public UpdateStaffCommandHandler(ISupermarketDbContext pContext, IMapper pMapper,
            IMediator pMediator, ICurrentUserService pCurrentUserService) :
            base(pContext, pMapper, pMediator, pCurrentUserService)
        {
        }
    }

}
