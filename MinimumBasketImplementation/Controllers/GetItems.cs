using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace MinimumBasketImplementation
{
    public static class GetItems
    {
        public static async Task<(bool success, string? error, Basket? basket)> GetItemsFromBasket(ClaimsPrincipal user)
        {
            try
            {
                var (isValid, errorMessage, userId) = AuthHelper.ValidateUser(user);
                if (!isValid)
                    return (false, errorMessage, null);

                IMemoryCache? cache = BasketCache.Cache;
                
                if(!BasketHelper.TryGetBasketFromCache(cache, userId, out string cacheKey))
                    return (false, "Basket not found for the user.", null);


                if (cache.TryGetValue(cacheKey, out object? basketObj) && basketObj is Basket basket && basket is not null)
                {
                    if(basket.UserID != userId)
                        return (false, "User is not authorized to access this basket.", null);

                    await Task.CompletedTask;
                    return (true, null, basket);
                }
                else
                {
                    return (false, "Basket not found.", null);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
