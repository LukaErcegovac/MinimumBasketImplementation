using System.Security.Claims;

namespace MinimumBasketImplementation
{
    public static class AuthHelper
    {
        public static (bool isValid, string errorMessage, int userId) ValidateUser(ClaimsPrincipal? user)
        {
            if (user?.Identity?.IsAuthenticated != true)
                return (false, "User is not authenticated.", 0);

            string? userIDClaim = user.FindFirst("userId")?.Value;
            if (!int.TryParse(userIDClaim, out int userId))
                return (false, "Invalid user ID claim.", 0);

            return (true, string.Empty, userId);
        }
    }
}
