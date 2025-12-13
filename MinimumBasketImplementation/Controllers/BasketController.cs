using Microsoft.Extensions.Caching.Memory;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace MinimumBasketImplementation
{
    public static class BasketController
    {
        public static async Task<(bool success, string error, Basket? basket)> AddItem(ItemDTO dto, Guid? basketId, ClaimsPrincipal? user)
        {
            try
            {
                if (user?.Identity?.IsAuthenticated != true)
                    return (false, "User is not authenticated.", null);

                var userIDClaim = user.FindFirst("userId")?.Value;
                if (!int.TryParse(userIDClaim, out int userId))
                    return (false, "Invalid user ID claim.", null);

                var id = basketId ?? Guid.NewGuid();
                string cacheKey = $"basket:{id}";

                var cache = BasketCache.Cache;

                if (cache.TryGetValue(cacheKey, out object? basketObj) && basketObj is Basket basket && basket is not null)
                {
                    if(basket.Items == null)
                        basket.Items = new List<Item>();

                    var existingItem = basket.Items.FirstOrDefault(i => i.ID == dto.id);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += dto.quantity;
                    }
                    else
                    {
                        var itemToAdd = new Item(dto.id, dto.name ?? string.Empty, dto.price, dto.quantity);
                        basket.Items.Add(itemToAdd);
                    }
                }
                else
                {
                    var newItem = new Item(dto.id, dto.name ?? string.Empty, dto.price, dto.quantity);
                    var items = new List<Item> { newItem };
                    var newBasket = new Basket(id, items, userId);
                    basket = newBasket;
                }

                cache.Set(cacheKey, basket, new MemoryCacheEntryOptions
                {
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });

                await Task.CompletedTask;
                return (true, string.Empty, basket);

            }
            catch (Exception ex)
            {
                return (false, ex.Message, null);
            }
        }
    }
}
