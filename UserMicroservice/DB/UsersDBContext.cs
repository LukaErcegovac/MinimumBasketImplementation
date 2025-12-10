using Microsoft.EntityFrameworkCore;

namespace UserMicroservice
{
    public class UsersDBContext : DbContext
    {

        public UsersDBContext() { }
        public UsersDBContext(DbContextOptions<UsersDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=users.db");
            }
        }
    }
}
