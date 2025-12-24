using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TodoListAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Priorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RoleId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TodoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PriorityId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TodoItems_Priorities_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TodoItems_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Critical" },
                    { 2, "High" },
                    { 3, "Medium" },
                    { 4, "Low" },
                    { 5, "None" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "admin" },
                    { 2, "user" },
                    { 3, "guest" },
                    { 4, "moderator" },
                    { 5, "superuser" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "Login", "PasswordHash", "RoleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 24, 12, 37, 11, 962, DateTimeKind.Utc).AddTicks(3730), "admin@example.com", "admin", "$2a$11$gHz6CDY3qFxYid3ddhl1XOoNbY2L5pKmA88p4PCsIVLcIaozmwFUu", 1 },
                    { 2, new DateTime(2025, 12, 24, 12, 37, 12, 136, DateTimeKind.Utc).AddTicks(125), "user1@example.com", "user1", "$2a$11$Go9gf7B8fACKkniSkC8jLOYpi8Vyfi8ovFKJ3KzSSR5.AZG3xUSZa", 2 },
                    { 3, new DateTime(2025, 12, 24, 12, 37, 12, 265, DateTimeKind.Utc).AddTicks(9892), "user2@example.com", "user2", "$2a$11$rL22Dk3Lk6lamISmU7V81OZwSPma2G6Bp2w9hzFMVMkOXdH13BdH2", 2 },
                    { 4, new DateTime(2025, 12, 24, 12, 37, 12, 388, DateTimeKind.Utc).AddTicks(2511), "user3@example.com", "user3", "$2a$11$ZW1r.PlyOlTmwUpXRELBSeKWtOLywuiw8m5xkIZUmel03KOmkTIYC", 2 },
                    { 5, new DateTime(2025, 12, 24, 12, 37, 12, 510, DateTimeKind.Utc).AddTicks(5346), "user4@example.com", "user4", "$2a$11$HLx95fM1/.ua5KrrVfx3ReftNhvho0vkJOAIDj8zLfPxhW5PTBUDe", 2 }
                });

            migrationBuilder.InsertData(
                table: "TodoItems",
                columns: new[] { "Id", "CreatedAt", "Description", "DueDate", "IsCompleted", "PriorityId", "Title", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 24, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(2182), "Desc 1", new DateTime(2025, 12, 25, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(2324), false, 1, "Task 1", 1 },
                    { 2, new DateTime(2025, 12, 24, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(3071), "Desc 2", new DateTime(2025, 12, 26, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(3071), false, 2, "Task 2", 2 },
                    { 3, new DateTime(2025, 12, 24, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(3075), "Desc 3", new DateTime(2025, 12, 24, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(3076), true, 3, "Task 3", 3 },
                    { 4, new DateTime(2025, 12, 24, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(3077), "Desc 4", new DateTime(2025, 12, 27, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(3078), false, 4, "Task 4", 4 },
                    { 5, new DateTime(2025, 12, 24, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(3080), "Desc 5", new DateTime(2025, 12, 29, 12, 37, 12, 626, DateTimeKind.Utc).AddTicks(3080), false, 5, "Task 5", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_PriorityId",
                table: "TodoItems",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_UserId",
                table: "TodoItems",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItems");

            migrationBuilder.DropTable(
                name: "Priorities");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
