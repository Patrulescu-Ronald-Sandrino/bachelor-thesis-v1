using Microsoft.EntityFrameworkCore;

namespace bll.Data;

public static class QueryableExtensions
{
    public static IQueryable<T> SetTracking<T>(this IQueryable<T> queryable, bool tracking = false) where T : class
    {
        return tracking ? queryable.AsTracking() : queryable.AsNoTracking();
    }
}