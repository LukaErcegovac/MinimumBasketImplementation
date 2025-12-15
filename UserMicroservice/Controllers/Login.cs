using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace UserMicroservice
{
    public static class Login
    {
        public static async Task<(bool success, string? token, string? error)> LoginUser(
                UsersDBContext db,
                string email,
                string password,
                string jwtKey,
                string jwtIssuer,
                string jwtAudience,
                int jwtExpireMinutes)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return (false, null, "Email and password cannot be empty.");

                User? user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                    return (false, null, "Invalid credentials.");

                bool passwordMatches = BCrypt.Net.BCrypt.Verify(password, user.Password);
                if (!passwordMatches)
                    return (false, null, "Invalid credentials.");

                string? token = GenerateJwtToken(user, jwtKey, jwtIssuer, jwtAudience, jwtExpireMinutes);
                return (true, token, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        private static string GenerateJwtToken(User user, string jwtKey, string jwtIssuer, string jwtAudience, int jwtExpireMinutes)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? throw new ArgumentNullException(nameof(jwtKey))));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("userId", user.ID.ToString()),
                new Claim("firstName", user.FirstName ?? string.Empty),
                new Claim("lastName", user.LastName ?? string.Empty)
            };

            JwtSecurityToken? token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtExpireMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
