using System.Linq.Expressions;

namespace PrismaPrimeInvest.Application.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> OrderByDynamic<T>(
        this IQueryable<T> query,
        string orderBy,
        bool isAscending = true)
    {
        if (string.IsNullOrEmpty(orderBy))
        {
            throw new ArgumentException("Order by field cannot be null or empty", nameof(orderBy));
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, orderBy);
        var lambda = Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), parameter);

        if (isAscending)
        {
            return query.OrderBy(lambda);
        }
        else
        {
            return query.OrderByDescending(lambda);
        }
    }
}
