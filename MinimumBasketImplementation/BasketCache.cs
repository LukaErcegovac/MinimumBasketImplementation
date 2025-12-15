using Microsoft.Extensions.Caching.Memory;

namespace MinimumBasketImplementation
{
    public static class BasketCache
    {
        public static IMemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions());
    }
}
