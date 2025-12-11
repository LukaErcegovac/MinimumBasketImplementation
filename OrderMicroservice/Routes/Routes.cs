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
        }
    }
}
