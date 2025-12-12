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
                var user = httpContext.User;

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
        }
    }
}
