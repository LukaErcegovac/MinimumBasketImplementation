using System.Security.Claims;

namespace MinimumBasketImplementation
{
    public static class Routes
    {
        public static void BasketRoutes(this WebApplication app)
        {
            app.MapPost("/basket/items", async (ItemDTO dto, Guid? basketId, HttpContext httpContext) =>
            {
                ClaimsPrincipal? user = httpContext.User;
                var (success, error, basket) = await BasketController.AddItem(dto, basketId, user);
                if (success)
                {
                    return Results.Ok(basket);
                }
                else
                {
                    return Results.BadRequest(new { error });
                }
            }).RequireAuthorization();

            app.MapDelete("/basket/remove/item", async (int itemId, HttpContext httpContext) =>
            {
                ClaimsPrincipal? user = httpContext.User;
                var (success, error) = await RemoveItem.RemoveItemFromBasket(itemId, user);
                if (success)
                {
                    return Results.Ok(new { message = "Item removed successfully." });
                }
                else
                {
                    return Results.BadRequest(new { error });
                }
            }).RequireAuthorization();

            app.MapDelete("/basket/delete", async (HttpContext httpContext) =>
            {
                ClaimsPrincipal? user = httpContext.User;
                var (success, error) = await RemoveItem.RemoveAllItemsFromBasket(user);
                if (success)
                {
                    return Results.Ok(new { message = "All items removed successfully." });
                }
                else
                {
                    return Results.BadRequest(new { error });
                }
            }).RequireAuthorization();

            app.MapGet("/basket/items", async (HttpContext httpContext) =>
            {
                ClaimsPrincipal? user = httpContext.User;
                var (success, error, basket) = await GetItems.GetItemsFromBasket(user);
                if (success && basket is not null)
                {
                    return Results.Ok(basket);
                }
                else
                {
                    return Results.BadRequest(new { error });
                }
            }).RequireAuthorization();

            app.MapPost("/basket/confirm", async (HttpContext httpContext) =>
            {
                ClaimsPrincipal? user = httpContext.User;
                var (success, error) = await ConfirmBasket.ConfirmUserBasket(user, app.Configuration);
                if (success)
                {
                    return Results.Ok(new { message = "Basket confirmed successfully." });
                }
                else
                {
                    return Results.BadRequest(new { error });
                }
            }).RequireAuthorization();
        }
    }
}
