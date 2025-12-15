using System.Security.Claims;

namespace OrderMicroservice
{
    public static class Routes
    {
        public static void OrderRoutes(this WebApplication app)
        {
            app.MapPost("/orders", async (OrderDBContex db, CreateOrderDTO dto) =>
            {
                var (success, order, error) = await CreateOrder.CreateOrders(db, dto);
                if (success)
                {
                    return Results.Created($"/orders/{order.ID}", order);
                }
                else
                {
                    return Results.BadRequest(new { error });
                }
            });

            app.MapDelete("/orders/{orderId:int}", async (OrderDBContex db, int orderId, HttpContext httpContext) =>
            {
                ClaimsPrincipal? user = httpContext.User;

                if(user?.Identity?.IsAuthenticated != true)
                    return Results.Unauthorized();

                var (success, error) = await DeleteOrder.RemoveOrder(db, orderId, user);
                if (success)
                {
                    return Results.NoContent();
                }
                else
                {
                    return Results.BadRequest(new { error });
                }
            }).RequireAuthorization();

            app.MapGet("/orders/user", async (OrderDBContex db, HttpContext httpContext) =>
            {
                ClaimsPrincipal? user = httpContext.User;
                var (success, error, orders) = await GetOrders.GetUsersOrders(db, user);
                if (success)
                {
                    return Results.Ok(orders);
                }
                else
                {
                    return Results.BadRequest(new { error });
                }
            }).RequireAuthorization();

            app.MapGet("/orders/all", async (OrderDBContex db) =>
            {
                var (success, error, orders) = await GetOrders.GetAllOrders(db);
                if (success)
                {
                    return Results.Ok(orders);
                }
                else
                {
                    return Results.BadRequest(new { error });
                }
            });
        }
    }
}
