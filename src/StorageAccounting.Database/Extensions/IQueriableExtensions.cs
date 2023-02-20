using System;
using System.Linq;
using System.Linq.Expressions;

namespace StorageAccounting.Database.Extensions
{
    internal static class IQueriableExtensions
    {
        static IQueryable<T> OptionalSkip<T>(this IQueryable<T> queriable, int? start) =>
            IsSkip(start) ? queriable.Skip(start!.Value) : queriable;

        static IQueryable<T> OptionalTake<T>(this IQueryable<T> queriable, int? size) =>
            IsTake(size) ? queriable.Take(size!.Value) : queriable;

        public static IQueryable<TEntity> OptionalPagination<TEntity, TKey>(this IQueryable<TEntity> queriable,
            Expression<Func<TEntity, TKey>> optionalOrderBy,
            int? start,
            int? size)
        {
            if(IsSkip(start) || IsTake(size))
            {
                return queriable.OrderBy(optionalOrderBy)
                    .OptionalSkip(start)
                    .OptionalTake(size);
            }

            return queriable;
        }

        static bool IsSkip(int? start) => start is not null && start > 0;
        static bool IsTake(int? size) => size is not null && size > 0;
    }
}