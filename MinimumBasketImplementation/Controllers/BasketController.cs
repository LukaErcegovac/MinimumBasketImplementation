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
                var (isValid, errorMessage, userId) = AuthHelper.ValidateUser(user);
                if (!isValid)
                    return (false, errorMessage, null);

                IMemoryCache? cache = BasketCache.Cache;

                string userKey = $"userBasket:{userId}";
                Guid id;

                if(cache.TryGetValue(userKey, out object? userObj) && userObj is Guid baskeId)
                {
                    id = baskeId;
                }
                else
                {
                    id = basketId ?? Guid.NewGuid();
                }

                string cacheKey = $"basket:{id}";

                if (cache.TryGetValue(cacheKey, out object? basketObj) && basketObj is Basket basket && basket is not null)
                {
                    if(basket.UserID != userId)
                        return (false, "User is not authorized to modify this basket.", null);

                    if (basket.Items == null)
                        basket.Items = new List<Item>();

                    Item? existingItem = basket.Items.FirstOrDefault(i => i.ID == dto.id);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += dto.quantity;
                    }
                    else
                    {
                        Item itemToAdd = new Item(dto.id, dto.name ?? string.Empty, dto.price, dto.quantity);
                        basket.Items.Add(itemToAdd);
                    }
                }
                else
                {
                    Item newItem = new Item(dto.id, dto.name ?? string.Empty, dto.price, dto.quantity);
                    List<Item> items = new List<Item> { newItem };
                    Basket newBasket = new Basket(id, items, userId);
                    basket = newBasket;

                    cache.Set(userKey, id, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(30)
                    });
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
