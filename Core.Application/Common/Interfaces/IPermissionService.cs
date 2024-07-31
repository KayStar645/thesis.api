namespace Core.Application.Common.Interfaces
{
    public interface IPermissionService
    {
        Task Create(List<string> pPermissions);
    }
}
