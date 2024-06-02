using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToDoListMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class Inital : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskJobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskJobs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TaskJobs",
                columns: new[] { "Id", "CreatedDate", "DeletedDate", "Description", "IsActive", "IsDeleted", "Priority", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 6, 2, 12, 50, 55, 84, DateTimeKind.Local).AddTicks(9479), null, "Elektrik Makinaları 2 ödevi var.", true, false, 1, "Ödevi Yap" },
                    { 2, new DateTime(2024, 6, 2, 12, 50, 55, 84, DateTimeKind.Local).AddTicks(9482), null, "Son haftanın staj defterinde eksikler var.", true, false, 2, "Staj defterini tamamla" },
                    { 3, new DateTime(2024, 6, 2, 12, 50, 55, 84, DateTimeKind.Local).AddTicks(9484), null, "OBS'den bir mesaj gelebilir.", true, false, 3, "OBS'yi kontrol et" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskJobs");
        }
    }
}
