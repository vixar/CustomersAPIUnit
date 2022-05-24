using Microsoft.EntityFrameworkCore;

namespace Application.Extentions;


    public static class QueryableExtensions
    {
        public static async Task<ApiResponse.PaginatedResponse<T>> ToPaginatedListAsync<T>(this IQueryable<T> source,
            int pageNumber, int pageSize) where T : class
        {
            //Throw.Exception.IfNull(source, nameof(source));
            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 10 : pageSize;
            int count = source.Count();
            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            List<T> items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            return ApiResponse.PaginatedResponse<T>.Success(items, count, pageNumber, pageSize);
        }
    }

