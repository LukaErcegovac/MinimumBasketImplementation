using Microsoft.Extensions.Caching.Memory;

namespace MinimumBasketImplementation
{
    public static class BasketHelper
    {
        public static bool TryGetBasketFromCache(IMemoryCache cache, int userId, out string cacheKey)
        {
            cacheKey = string.Empty;
            string userKey = $"userBasket:{userId}";

            if (!cache.TryGetValue(userKey, out object? basketIdObj) || basketIdObj is not Guid basketId)
            {
                return false;
            }

            cacheKey = $"basket:{basketId}";
            return true;
        }
    }
}
