using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace OrderMicroservice
{
    public static class GetOrders
    {
        public static async Task<(bool success, string? error, List<Order>? orders)> GetUsersOrders(OrderDBContex db, ClaimsPrincipal user)
        {
            try
            {
                if (user?.Identity?.IsAuthenticated != true)
                    return (false, "User is not authenticated.", null);

                string? userIDClaim = user.FindFirst("userId")?.Value;
                if (!int.TryParse(userIDClaim, out int userId))
                    return (false, "Invalid user ID claim.", null);

                var orders = await db.Orders.Where(o => o.UserID == userId).ToListAsync();
                var orderItems = await db.OrderItems.Where(oi => orders.Select(o => o.ID).Contains(oi.OrderID)).ToListAsync();
                foreach (var order in orders)
                {
                    order.OrderItems = orderItems.Where(oi => oi.OrderID == order.ID).ToList();
                }

                return (true, null, orders);
            }
            catch (Exception ex)
            {
                return (false, $"Error validating user: {ex.Message}", null);
            }
        }

        public static async Task<(bool success, string? error, List<Order>? orders)> GetAllOrders(OrderDBContex db)
        {
            try
            {
                var orders = await db.Orders.ToListAsync();
                var orderItems = await db.OrderItems.ToListAsync();
                foreach (var order in orders)
                {
                    order.OrderItems = orderItems.Where(oi => oi.OrderID == order.ID).ToList();
                }
                return (true, null, orders);
            }
            catch (Exception ex)
            {
                return (false, $"Error retrieving orders: {ex.Message}", null);
            }
        }
    }
}
