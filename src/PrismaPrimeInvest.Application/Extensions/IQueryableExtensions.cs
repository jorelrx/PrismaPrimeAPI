using System.Linq.Expressions;

namespace PrismaPrimeInvest.Application.Extensions;

public static class IQueryableExtensions
{
    public static IQueryable<T> OrderByDynamic<T>(
        this IQueryable<T> query,
        string orderBy,
        string sortDirection = "asc")
    {
        if (string.IsNullOrWhiteSpace(orderBy))
        {
            throw new ArgumentException("Order by field cannot be null or empty", nameof(orderBy));
        }

        if (!sortDirection.Equals("asc", StringComparison.CurrentCultureIgnoreCase) && !sortDirection.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
        {
            throw new ArgumentException("Sort direction must be 'asc' or 'desc'", nameof(sortDirection));
        }

        var parameter = Expression.Parameter(typeof(T), "x");
        var property = Expression.Property(parameter, orderBy);
        var propertyType = property.Type;

        var lambda = Expression.Lambda(property, parameter);

        string methodName = sortDirection.Equals("asc", StringComparison.CurrentCultureIgnoreCase) ? "OrderBy" : "OrderByDescending";
        
        var queryExpression = Expression.Call(
            typeof(Queryable),
            methodName,
            [typeof(T), propertyType],
            query.Expression,
            Expression.Quote(lambda));

        return query.Provider.CreateQuery<T>(queryExpression);
    }
}
