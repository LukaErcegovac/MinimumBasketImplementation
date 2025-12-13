namespace MinimumBasketImplementation
{
    public static class Routes
    {
        public static void BasketRoutes(this WebApplication app)
        {
            app.MapPost("/basket/items", async (ItemDTO dto, Guid? basketId, HttpContext httpContext) =>
            {
                var user = httpContext.User;
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
        }
    }
}
