using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class UpdatedAdminSeeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "4e4d6ea7-9024-4945-8607-917db171ac0e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "11dcc865-0d9b-406a-9569-8094dc2bd47f");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "01cfaac9-4411-42a0-95dc-621036542238");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "0e5be9d8-9510-4b54-9135-45398e70caae");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Email", "LastName", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "37de0cc4-629f-467b-bd2d-9f5046355ff2", "admin.mail@gmail.com", "AdminLastName", "ADMIN.MAIL@GMAIL.COM", "AQAAAAEAACcQAAAAEAELkWoSX6FUrJBWeY/0rTActcHmJRAdVmCaWb7++7Si7cpA3PUIpnNK39TtZUQLhg==", "cfefc117-da9f-45c8-838c-ea80f83aee22" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c5bd58fb-8ffc-4d0d-a0e6-6984341f953b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "5b648256-e392-4229-8342-1823c71f220d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "66c9f11a-a0a8-497c-b479-2cf0b532299d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "04ca866f-b124-4224-bef7-1d59f32fcd92");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Email", "LastName", "NormalizedEmail", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5787d213-be1a-4674-9b26-7ff3ad1ca450", null, null, null, "AQAAAAEAACcQAAAAEIkywEvc0IX1R4uKhvfpNC7d7Up2cUkd1fY+8kWmSRHR5iTd+pm+Jaw84CDaz4eQNA==", "a9cb6b4a-42be-4f20-8a21-c922a2455258" });
        }
    }
}
