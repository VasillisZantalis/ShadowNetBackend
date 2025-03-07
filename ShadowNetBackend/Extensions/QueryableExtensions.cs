using ShadowNetBackend.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace ShadowNetBackend.Extensions;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string fieldName, SortingOrder order)
    {
        if (string.IsNullOrWhiteSpace(fieldName))
            return query; // No sorting applied if field is empty

        var entityType = typeof(T);
        var property = entityType.GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property == null)
            return query; // Ignore if field is not found

        var parameter = Expression.Parameter(entityType, "x");
        var propertyAccess = Expression.Property(parameter, property);
        var orderByExpression = Expression.Lambda(propertyAccess, parameter);

        string methodName = order == SortingOrder.Asc ? "OrderBy" : "OrderByDescending";

        var resultExpression = Expression.Call(typeof(Queryable), methodName,
            [entityType, property.PropertyType],
            query.Expression, Expression.Quote(orderByExpression));

        return query.Provider.CreateQuery<T>(resultExpression);
    }
}
