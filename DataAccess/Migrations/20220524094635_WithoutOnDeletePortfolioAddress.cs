using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class WithoutOnDeletePortfolioAddress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "227abc7a-2edf-487c-8569-52d7891e35a7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "96ea0a31-c077-4e62-8e12-16b9da08c3ca");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "792a5d38-25df-4275-af3c-10cf0cbbaeeb");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "76912dd8-aaa9-43ef-ba1f-fef762b7bb9d");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "43461189-c21c-4aab-8f05-38035ebca55e", "AQAAAAEAACcQAAAAEAKzZdnuoQyTVaDEL1ni/QxbDuPffkIqgEfQw06oQAYwuZQf2v3dCD1XJNCVy3C7oQ==", "7817bca0-9249-4658-84dd-de09c8add482" });
        }
    }
}
