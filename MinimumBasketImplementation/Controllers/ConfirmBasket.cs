using Microsoft.Extensions.Caching.Memory;
using MinimumBasketImplementation.DTO;
using System.Security.Claims;

namespace MinimumBasketImplementation
{
    public static class ConfirmBasket
    {
        public static async Task<(bool success, string? error)> ConfirmUserBasket(ClaimsPrincipal user, IConfiguration configuration)
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
                        return (false, "User is not authorized to confirm this basket.");

                    string? orderServiceUrl = configuration["OrderMicroservice"];

                    if (string.IsNullOrEmpty(orderServiceUrl))
                        return (false, "Order microservice URL is not configured.");

                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.BaseAddress = new Uri(orderServiceUrl);
                        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

                        var order = new CreateOrderDTO
                        {
                            UserID = basket.UserID,
                            OrderItems = basket.Items?.Select(i => new CreateOrderItemsDTO
                            {
                                ProductID = i.ID,
                                UnitPrice = i.Price,
                                Quantity = i.Quantity
                            }).ToList() ?? new List<CreateOrderItemsDTO>()
                        };

                        var response = await httpClient.PostAsJsonAsync("/orders", order);

                        if (!response.IsSuccessStatusCode)
                            return (false, $"Failed to confirm basket. Status Code: {response.StatusCode}");
                    }

                        basket.Items?.Clear();
                    cache.Set(cacheKey, basket, new MemoryCacheEntryOptions
                    {
                        SlidingExpiration = TimeSpan.FromMinutes(30)
                    });

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
