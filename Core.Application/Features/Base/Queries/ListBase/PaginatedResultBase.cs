using Sieve.Models;
using Sieve.Services;

namespace Core.Application.Features.Base.Queries.GetListBase
{
    public static class PaginatedResultBase
    {
        public async static Task<int> CountApplySieveAsync<T>(ISieveProcessor pIsieveProcessor, SieveModel pSieve, IQueryable<T> pQuery)
        {
            SieveModel sieveModel = new SieveModel();
            sieveModel.Filters = pSieve.Filters;
            sieveModel.Sorts = pSieve.Sorts;
            var queryCount = pIsieveProcessor.Apply(sieveModel, pQuery);
            int totalCount = await queryCount.CountAsync();

            return totalCount;
        }
    }
}
