using Microsoft.EntityFrameworkCore;

namespace TodoApp.Models
{
    public class TodoDbContext: DbContext
    {
        public TodoDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Todo> Todos { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<UserLoginHistory> UserLoginHistories { get; set; }
    }
}
