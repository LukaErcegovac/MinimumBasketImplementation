namespace OrderMicroservice
{
    public class CreateOrder
    {
        public static async Task<(bool success, Order order, string? error)> CreateOrders(OrderDBContex db, CreateOrderDTO dto)
        {
            try
            {
                if (db is null) return (false, null!, "Database context is null.");
                if (dto is null) return (false, null!, "Order data is null.");

                Order newOrder = new Order
                {
                    UserID = dto.userID,
                    OrderDate = DateTime.UtcNow,
                    OrderItems = dto.orderItems.Select(oi => new OrderItem
                    {
                        ProductID = oi.productID,
                        Quantity = oi.quantity,
                        UnitPrice = oi.unitPrice
                    }).ToList()
                };

                db.Orders.Add(newOrder);
                await db.SaveChangesAsync();

                return (true, newOrder, null);
            }
            catch (Exception ex)
            {
                return (false, null!, ex.Message);
            }
        }
    }
}
