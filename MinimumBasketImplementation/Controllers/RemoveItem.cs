using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace MinimumBasketImplementation
{
    public static class RemoveItem
    {
        public static async Task<(bool success, string? error)> RemoveItemFromBasket(int itemId, ClaimsPrincipal user)
        {
            try
            {
                var (isValid, errorMessage, userId) = AuthHelper.ValidateUser(user);
                if (!isValid)
                    return (false, errorMessage);

                IMemoryCache? cache = BasketCache.Cache;

                if (!BasketHelper.TryGetBasketFromCache(cache, userId, out string cacheKey))
                    return (false, "Basket not found for the user.");

                if (cache.TryGetValue(cacheKey, out object? basketObj) && basketObj is Basket basket && basket is not null)
                {
                    if(basket.UserID != userId)
                        return (false, "User is not authorized to modify this basket.");

                    var itemToRemove = basket.Items?.FirstOrDefault(i => i.ID == itemId);
                    if (itemToRemove != null)
                    {
                        basket.Items?.Remove(itemToRemove);
                        cache.Set(cacheKey, basket, new MemoryCacheEntryOptions
                        {
                            SlidingExpiration = TimeSpan.FromMinutes(30)
                        });
                        await Task.CompletedTask;
                        return (true, null);
                    }
                    else
                    {
                        return (false, "Item not found in the basket.");
                    }
                }
                else
                {
                    return (false, "Basket not found.");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public static async Task<(bool success, string? error)> RemoveAllItemsFromBasket(ClaimsPrincipal user)
        {
            try
            {
                var (isValid, errorMessage, userId) = AuthHelper.ValidateUser(user);
                if (!isValid)
                    return (false, errorMessage);

                IMemoryCache? cache = BasketCache.Cache;

                if (!BasketHelper.TryGetBasketFromCache(cache, userId, out string cacheKey))
                    return (false, "Basket not found for the user.");
                
                if (cache.TryGetValue(cacheKey, out object? basketObj) && basketObj is Basket basket && basket is not null)
                {
                    if (basket.UserID != userId)
                        return (false, "User is not authorized to modify this basket.");

                    basket.Items?.Clear();
                    cache.Remove(cacheKey);

                    await Task.CompletedTask;
                    return (true, null);
                }
                else
                {
                    return (false, "Basket not found.");
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
