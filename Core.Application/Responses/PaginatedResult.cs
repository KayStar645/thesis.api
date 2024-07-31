using Microsoft.AspNetCore.Http;

namespace Core.Application.Responses
{
    public class Extra
    {
        public int? CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
        public int? PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public Extra(int pCount, int? pCurrentPage, int? pPageSize)
        {
            CurrentPage = pCurrentPage;
            PageSize = pPageSize;
            TotalPages = (int)Math.Ceiling(pCount / (double)pPageSize);
            TotalCount = pCount;
        }

    }


    public class PaginatedResult<T> : Result<T>
    {
        public Extra Extra { get; set; }

        public PaginatedResult(bool pSucceeded, int pCode, T pData = default, List<string> pMessages = null,
            int pCount = 0, int? pCurrentPage = 1, int? pPageSize = 30)
        {
            Data = pData;
            Succeeded = pSucceeded;
            Messages = pMessages;
            Code = pCode;
            Extra = new Extra(pCount, pCurrentPage, pPageSize);
        }

        public static PaginatedResult<T> Create(T pData, int pCount, int pPageNumber, int pPageSize, int pCode)
        {
            return new PaginatedResult<T>(true, pCode, pData, null, pCount, pPageNumber, pPageSize);
        }

        public static PaginatedResult<T> Success(T data, int count, int? pageNumber = 1, int? pageSize = 30)
        {
            return new PaginatedResult<T>(true, StatusCodes.Status200OK, data, null, count, pageNumber, pageSize);
        }

        public static PaginatedResult<T> Failure(int pCode, List<string> pMessages)
        {
            return new PaginatedResult<T>(false, pCode, default, pMessages);
        }
    }
}
