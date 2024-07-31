using Core.Application.Services;

namespace Core.Application.Common.Interfaces
{
    public interface ICLHUIService
    {
        Task<List<CLHUIs>> RunAlgorithm(int pMinUtil, int? pMonth, int? pYear);

        Task<CLHUIs> Detail(int id);
    }
}
