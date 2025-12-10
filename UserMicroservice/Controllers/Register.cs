namespace UserMicroservice
{
    public static class Register
    {
        public static async Task<(bool success, int? createdUserID, string? error)> RegisterUser(
                UsersDBContext db,
                string firstName,
                string lastName,
                string email,
                string password)
        {
            try
            {
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    return (false, null, "Email and password cannot be empty.");

                if (db.Users.Any(user => user.Email == email))
                    return (false, null, "A user with this email already exists.");

                User newUser = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
                };

                newUser.Password = BCrypt.Net.BCrypt.HashPassword(password);
                db.Users.Add(newUser);
                await db.SaveChangesAsync();

                return (true, newUser.ID, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}
