using Microsoft.EntityFrameworkCore;
using System.Data;
using TodoListAPI.Models;
using TodoListAPI.Models.DTO;
namespace TodoListAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<TodoItem> TodoItems { get; set; } = null!;
        public DbSet<Priority> Priorities { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связи
            modelBuilder.Entity<TodoItem>()
            .HasOne(t => t.User)
            .WithMany(u => u.TodoItems)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<TodoItem>()
            .HasOne(t => t.Priority)
            .WithMany(p => p.TodoItems)
            .HasForeignKey(t => t.PriorityId)
            .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
            // Seeding (по 5 записей на сущность) с фиксированными хэшами и датами
            modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "admin" },
            new Role { Id = 2, Name = "user" },
            new Role { Id = 3, Name = "guest" },
            new Role { Id = 4, Name = "moderator" },
            new Role { Id = 5, Name = "superuser" }
            );
            modelBuilder.Entity<Priority>().HasData(
            new Priority { Id = 1, Name = "Critical" },
            new Priority { Id = 2, Name = "High" },
            new Priority { Id = 3, Name = "Medium" },
            new Priority { Id = 4, Name = "Low" },
            new Priority { Id = 5, Name = "None" }
            );
            modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Login = "admin", Email = "admin@example.com", PasswordHash = "$2b$11$bYk/LNyYBYFgcPGZbquXTuk5vgKesQpN2ZKn3SrDonXGDYjP2quUu", RoleId = 1 },
            new User { Id = 2, Login = "user1", Email = "user1@example.com", PasswordHash = "$2b$11$6WUoc/1z.RzubBth4fuA8.nl9ehcl91/V3EMm9Mx8niZ74sEc4DYC", RoleId = 2 },
            new User { Id = 3, Login = "user2", Email = "user2@example.com", PasswordHash = "$2b$11$mL3ta7NyrnQTtcnNAeaN6.3.KMQ8tIQ1Xzll7znEZh9ptvRhKecxi", RoleId = 2 },
            new User { Id = 4, Login = "user3", Email = "user3@example.com", PasswordHash = "$2b$11$B5fiUCraCkiJVgbxDHZqeen/GIoNleAADjffBs4fd0ZQEE2JUCx/i", RoleId = 2 },
            new User { Id = 5, Login = "user4", Email = "user4@example.com", PasswordHash = "$2b$11$xBlhk0cT1MKjTvPUqIYqd.CdCrqB0dvtoxFMRVfmSiGn.zL86O4IG", RoleId = 2 }
            );
            modelBuilder.Entity<TodoItem>().HasData(
            new TodoItem { Id = 1, Title = "Task 1", Description = "Desc 1", IsCompleted = false, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2023, 1, 2, 0, 0, 0, DateTimeKind.Utc), UserId = 1, PriorityId = 1 },
            new TodoItem { Id = 2, Title = "Task 2", Description = "Desc 2", IsCompleted = false, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2023, 1, 3, 0, 0, 0, DateTimeKind.Utc), UserId = 2, PriorityId = 2 },
            new TodoItem { Id = 3, Title = "Task 3", Description = "Desc 3", IsCompleted = true, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), UserId = 3, PriorityId = 3 },
            new TodoItem { Id = 4, Title = "Task 4", Description = "Desc 4", IsCompleted = false, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2023, 1, 4, 0, 0, 0, DateTimeKind.Utc), UserId = 4, PriorityId = 4 },
            new TodoItem { Id = 5, Title = "Task 5", Description = "Desc 5", IsCompleted = false, CreatedAt = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), DueDate = new DateTime(2023, 1, 6, 0, 0, 0, DateTimeKind.Utc), UserId = 5, PriorityId = 5 }
            );
        }
    }
}