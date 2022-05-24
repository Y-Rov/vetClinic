using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class PortfolioAddressWithoutID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "728191a1-6b20-43c7-b2ff-f57e72f11050");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "e04c1cd9-e346-4c8f-b54d-f1a58a60e909");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "bc9241cd-96fc-49de-a31e-8e0886199cce");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "d5f22341-35b0-4435-9ff6-7ca8a532d6c1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "9ff875ce-1a1e-4124-8f8a-94c96bf75b03", "AQAAAAEAACcQAAAAEBZx7BhB+tmD6RQwTtvpyhgQ10YJPI79/VsHzXfMVXj14vSSgZeCb6kF1sxMLAFxwg==", "95e2a2de-114a-47b0-8c29-9dbb5a50fa3a" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "d5afbc0b-15a2-497f-bac0-42a71f01a157");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "ad24bf56-43a5-47e0-893e-e260db72273f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "bf77de74-b81f-4260-b5f8-1cb91ab66573");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "36624077-3b93-48e8-88ef-68130e619974");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2699f26a-1b8e-40bc-9704-b5609036247f", "AQAAAAEAACcQAAAAEC3hQZNtMNZ9lh9Ic49DMF2TPLnefSWwqfPHsr+IuzU4wHyeLh6BeSBb29EhOKrOqA==", "d38e74f5-8d06-4d9f-a690-277b033197c4" });
        }
    }
}
