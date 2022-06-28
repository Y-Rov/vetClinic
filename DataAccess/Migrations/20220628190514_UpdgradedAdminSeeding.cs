using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class UpdgradedAdminSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "b771c2bb-df75-464d-9465-3415274b17e3");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "967fe632-cda2-4256-9372-83dd2a6e3dfe");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "e5c6ec60-544e-4f02-b395-aed8fdd33b22");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "9007621e-b681-4494-8251-a819e12c82a2");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Email", "LastName", "NormalizedEmail", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "f29f9b4c-7d86-4b49-92dc-31b3b74840ca", "admin.mail@gmail.com", "AdminLastName", "ADMIN.MAIL@GMAIL.COM", "AQAAAAEAACcQAAAAEHvCVgw0h624ljAuZsJeg1uFXDM/i+yJVvmDp9WenYmsZFBoEBuT3lhagVryM8kZTQ==", "00 000 000 0000", "ab4bdded-8336-4337-858c-5fa15ff26b01" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "72f880d7-60de-4cb7-9828-389a443f4a36");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "23df42e9-ebf9-4707-8a95-d6e6ca163824");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "4f636c4a-6c54-40b5-960b-f7e69d2fe72f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "b968f4be-f2e5-40d1-96f5-d8f87aa13371");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Email", "LastName", "NormalizedEmail", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "270a9998-361f-4ede-b59f-3ed2c45834d4", null, null, null, "AQAAAAEAACcQAAAAEKRkq7I0qLFP5ORAY8OUJAjgQSo4bELnkha8DXwN2FwoEW2/7CmMnt6bSTRBwxMhDA==", null, "07d066eb-727c-423b-97cf-b663bf90d8a8" });
        }
    }
}
