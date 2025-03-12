using System.Linq.Expressions;
using System.Reflection;

namespace ShadowNetBackend.Extensions;

public static class QueryableExtensions
{

    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query; // No sorting applied if orderBy is empty

        var entityType = typeof(T);
        var sortingFields = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);

        foreach (var field in sortingFields)
        {
            var parts = field.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var fieldName = parts[0]; // Extract field name
            var isDescending = parts.Length > 1 && parts[1].Equals("desc", StringComparison.OrdinalIgnoreCase);

            var property = entityType.GetProperty(fieldName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property == null)
                continue; // Skip invalid fields

            var parameter = Expression.Parameter(entityType, "x");
            var propertyAccess = Expression.Property(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);

            string methodName = isDescending ? "OrderByDescending" : "OrderBy";

            // If already sorted, use ThenBy / ThenByDescending
            if (query.Expression.Type.GetGenericArguments()[0] == typeof(IOrderedQueryable<T>))
            {
                methodName = isDescending ? "ThenByDescending" : "ThenBy";
            }

            var resultExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                [entityType, property.PropertyType],
                query.Expression,
                Expression.Quote(orderByExpression));

            query = query.Provider.CreateQuery<T>(resultExpression);
        }

        return query;
    }

    public static IQueryable<T> ApplyFiltering<T>(this IQueryable<T> query, string? propertyName, string? filterValue)
    {
        if (string.IsNullOrWhiteSpace(propertyName) || string.IsNullOrWhiteSpace(filterValue))
            return query;

        var entityType = typeof(T);
        var property = entityType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (property == null)
            return query; // Ignore invalid properties

        var parameter = Expression.Parameter(entityType, "x");
        var propertyAccess = Expression.Property(parameter, property);

        // Convert filterValue to match the property type (e.g., int, DateTime, etc.)
        object typedValue;
        try
        {
            typedValue = Convert.ChangeType(filterValue, property.PropertyType);
        }
        catch
        {
            return query; // Ignore if conversion fails
        }

        var constant = Expression.Constant(typedValue);
        var predicate = Expression.Equal(propertyAccess, constant); // x.Property == filterValue
        var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);

        return query.Where(lambda);
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int? pageSize, int? pageNumber)
    {
        if (pageSize.HasValue && pageNumber.HasValue)
        {
            int skip = (pageNumber.Value - 1) * pageSize.Value;
            return query.Skip(skip).Take(pageSize.Value);
        }
        return query;
    }
}
