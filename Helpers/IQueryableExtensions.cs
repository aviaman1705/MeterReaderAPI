using MeterReaderAPI.DTO;

namespace MeterReaderAPI.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO paginationDTO)
        {
            return queryable
                    .Skip((paginationDTO.PageIndex - 1) * paginationDTO.PageSize)
                    .Take(paginationDTO.PageSize);
        }
    }
}
