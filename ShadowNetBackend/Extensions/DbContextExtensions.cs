namespace ShadowNetBackend.Extensions;

public static class DbContextExtensions
{
    public static async Task<bool> ExistsAsync<TEntity>(this DbContext context, int id, CancellationToken cancellationToken) where TEntity : class
    {
        return await context.Set<TEntity>().AnyAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken);
    }

    public static async Task<bool> ExistsAsync<TEntity>(this DbContext context, string id, CancellationToken cancellationToken) where TEntity : class
    {
        return await context.Set<TEntity>().AnyAsync(e => EF.Property<string>(e, "Id") == id, cancellationToken);
    }

    public static async Task<bool> ExistsAsync<TEntity>(this DbContext context, Guid id, CancellationToken cancellationToken) where TEntity : class
    {
        return await context.Set<TEntity>().AnyAsync(e => EF.Property<Guid>(e, "Id") == id, cancellationToken);
    }
}