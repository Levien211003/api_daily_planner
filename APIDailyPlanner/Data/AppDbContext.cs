using Microsoft.EntityFrameworkCore;
using APIDailyPlanner.Models;

namespace APIDailyPlanner.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<APIDailyPlanner.Models.Task> Tasks { get; set; }
        public DbSet<TaskReminder> TaskReminders { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>()
                .HasKey(u => u.UserID); // Đảm bảo khai báo khóa chính
                                        // Đảm bảo khai báo khóa chính cho TaskReminder
            modelBuilder.Entity<TaskReminder>()
                .HasKey(tr => tr.ReminderID);
        }
    }
}
