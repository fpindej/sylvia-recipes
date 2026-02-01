namespace Recipes.Application.Caching.Constants;

public static class CacheKeys
{
    public static string User(Guid userId) => $"user:{userId}";
}
