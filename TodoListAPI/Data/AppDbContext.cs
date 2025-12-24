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

            // Seeding (по 5 записей на сущность)
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
                new User { Id = 1, Login = "admin", Email = "admin@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("adminpass"), RoleId = 1 },
                new User { Id = 2, Login = "user1", Email = "user1@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass1"), RoleId = 2 },
                new User { Id = 3, Login = "user2", Email = "user2@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass2"), RoleId = 2 },
                new User { Id = 4, Login = "user3", Email = "user3@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass3"), RoleId = 2 },
                new User { Id = 5, Login = "user4", Email = "user4@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("pass4"), RoleId = 2 }
            );

            modelBuilder.Entity<TodoItem>().HasData(
                new TodoItem { Id = 1, Title = "Task 1", Description = "Desc 1", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(1), UserId = 1, PriorityId = 1 },
                new TodoItem { Id = 2, Title = "Task 2", Description = "Desc 2", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(2), UserId = 2, PriorityId = 2 },
                new TodoItem { Id = 3, Title = "Task 3", Description = "Desc 3", IsCompleted = true, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow, UserId = 3, PriorityId = 3 },
                new TodoItem { Id = 4, Title = "Task 4", Description = "Desc 4", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(3), UserId = 4, PriorityId = 4 },
                new TodoItem { Id = 5, Title = "Task 5", Description = "Desc 5", IsCompleted = false, CreatedAt = DateTime.UtcNow, DueDate = DateTime.UtcNow.AddDays(5), UserId = 5, PriorityId = 5 }
            );
        }
    }
}
