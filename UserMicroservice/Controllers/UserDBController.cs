namespace UserMicroservice
{
    public static class UserDBController
    {
        public static void AddUser(User user)
        {
            using (var db = new UsersDBContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public static List<User> GetAllUsers()
        {
            using (var db = new UsersDBContext())
            {
                return db.Users.ToList();
            }
        }

        public static User? GetUserById(int id)
        {
            using (var db = new UsersDBContext())
            {
                return db.Users.Find(id);
            }
        }
    }
}
