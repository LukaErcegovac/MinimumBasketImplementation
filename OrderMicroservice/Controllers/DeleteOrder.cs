using System.Security.Claims;

namespace OrderMicroservice
{
    public static class DeleteOrder
    {
        public static async Task<(bool success, string? error)> RemoveOrder(OrderDBContex db, int orderId, ClaimsPrincipal? user)
        {
            try
            {
                if (db is null) return (false, "Database context is null.");
                if (user?.Identity?.IsAuthenticated != true)
                    return (false, "User is not authenticated.");

                string? userIDClaim = user.FindFirst("userId")?.Value;
                if(!int.TryParse(userIDClaim, out int userId))
                    return (false, "Invalid user ID claim.");

                Order? order = await db.Orders.FindAsync(orderId);
                if (order is null) return (false, "Order not found.");

                if (order.UserID != userId)
                    return (false, "User is not authorized to delete this order.");

                db.OrderItems.RemoveRange(db.OrderItems.Where(oi => oi.OrderID == orderId));
                db.Orders.Remove(order);
                await db.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }
    }
}
