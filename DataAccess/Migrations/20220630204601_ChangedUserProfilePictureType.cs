using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class ChangedUserProfilePictureType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e9c5351f-5789-48e2-bb4c-2e290ccc9225");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "edb63f2a-b150-47a9-8cc5-f8fba2773a23");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "0c9349ea-a826-4ac6-9894-69a06cb964ef");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 4,
                column: "ConcurrencyStamp",
                value: "13bcffc7-2246-488c-a1de-7c9c09b7a6c1");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Email", "LastName", "NormalizedEmail", "PasswordHash", "PhoneNumber", "SecurityStamp" },
                values: new object[] { "d40bd44f-933d-44d1-a1e7-7ea80d79ae23", "admin.mail@gmail.com", "AdminLastName", "ADMIN.MAIL@GMAIL.COM", "AQAAAAEAACcQAAAAEGZyIczkaVVM8Z7xYRRHE4ERqlkTMGT3ffnEU907C/xgTlG5M2ce5VOqpYEQkCsc7Q==", "00 000 000 0000", "b94e792b-c909-432b-818a-c029f23cd461" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
