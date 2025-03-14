using Zen.Domain.Common;

namespace Zen.Application.Common.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ExcludeDeleted<T>(this IQueryable<T> queryable)
        where T : class, ISoftDeletable
    {
        return queryable.Where(entity => !entity.IsDeleted);
    }
}
