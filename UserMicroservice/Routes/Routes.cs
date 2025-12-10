using System.Runtime.CompilerServices;

namespace UserMicroservice
{
    public static class Routes
    {
        public static void UserRoutes(this WebApplication app)
        {
            app.MapPost("/register", async (RegisterRequest req, UsersDBContext db) =>
            {
                var (success, createdUsedID, error) = await Register.RegisterUser(
                    db,
                    req.firstName,
                    req.lastName,
                    req.email,
                    req.password
                    );

                if (success)
                    return Results.Created($"/users/{createdUsedID}", new { ID = createdUsedID });

                return Results.BadRequest(new { Error = error });
            }).WithName("RegisterUser");

            app.MapPost("/login", async (LoginRequest req, UsersDBContext db) =>
            {
                var appConfig = app.Configuration;
                var key = appConfig["Jwt:Key"] ?? throw new ArgumentNullException("Jwt:Key");
                var issuer = appConfig["Jwt:Issuer"] ?? throw new ArgumentNullException("Jwt:Issuer");
                var audience = appConfig["Jwt:Audience"] ?? throw new ArgumentNullException("Jwt:Audience");
                var expireMinutes = int.Parse(appConfig["Jwt:ExpireMinutes"] ?? "60");

                var (success, token, error) = await Login.LoginUser(
                    db,
                    req.email,
                    req.password,
                    key,
                    issuer,
                    audience,
                    expireMinutes
                    );

                if (success)
                    return Results.Ok(new { Token = token });

                return Results.BadRequest(new { Error = error });
            }).WithName("LoginUser");
        }
    }
}
