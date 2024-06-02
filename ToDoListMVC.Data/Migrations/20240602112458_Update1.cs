using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ToDoListMVC.Data.Migrations
{
    /// <inheritdoc />
    public partial class Update1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "5a8a6859-93de-4273-b215-f2cd4bf18b09", new DateTime(2024, 6, 2, 14, 24, 58, 115, DateTimeKind.Local).AddTicks(6405), "AQAAAAIAAYagAAAAEBrVwkdUifV11KJdgIYB6rRfAeRfdXAuy93ZbHPasi7Qljq+9xL0FLl/mqaB2kcBvA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "e0175aae-9e6a-4021-8851-2315a8abc1cc", new DateTime(2024, 6, 2, 14, 24, 58, 187, DateTimeKind.Local).AddTicks(3734), "AQAAAAIAAYagAAAAEE4SmkL4aMqIcxRq56uWyE52OOtrEswjiQ/A8TcqcGs99z9WNN7fcHEI1XWlvNqBaA==" });

            migrationBuilder.UpdateData(
                table: "TaskJobs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 14, 24, 58, 115, DateTimeKind.Local).AddTicks(4131));

            migrationBuilder.UpdateData(
                table: "TaskJobs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 14, 24, 58, 115, DateTimeKind.Local).AddTicks(4134));

            migrationBuilder.UpdateData(
                table: "TaskJobs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 14, 24, 58, 115, DateTimeKind.Local).AddTicks(4136));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "7006e4f8-d977-4df6-9788-06450115ec6b", new DateTime(2024, 6, 2, 14, 0, 39, 479, DateTimeKind.Local).AddTicks(6448), "AQAAAAIAAYagAAAAENrs+ltDvGq4tb6aZyZY53XtuIkKG9Qf5xxggNRtNBjrDP3G1dwTsGyLxjRGd8W55g==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "2",
                columns: new[] { "ConcurrencyStamp", "CreatedDate", "PasswordHash" },
                values: new object[] { "4c26466f-4da4-4a87-a52a-b5a1a3368e97", new DateTime(2024, 6, 2, 14, 0, 39, 552, DateTimeKind.Local).AddTicks(3254), "AQAAAAIAAYagAAAAEAYuMS1XU5EojV2QvJTgEv1NiriTCmHfMlmZWyFfVaoWby/XE3GHkzZrhflHhRSsoA==" });

            migrationBuilder.UpdateData(
                table: "TaskJobs",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 14, 0, 39, 479, DateTimeKind.Local).AddTicks(3748));

            migrationBuilder.UpdateData(
                table: "TaskJobs",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 14, 0, 39, 479, DateTimeKind.Local).AddTicks(3751));

            migrationBuilder.UpdateData(
                table: "TaskJobs",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedDate",
                value: new DateTime(2024, 6, 2, 14, 0, 39, 479, DateTimeKind.Local).AddTicks(3753));
        }
    }
}
