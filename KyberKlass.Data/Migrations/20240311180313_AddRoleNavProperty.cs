using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class AddRoleNavProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a0638004-c5e4-4225-8a7d-7788e33e9629"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c01c17e1-0f32-40ca-a467-4379cde5bc98"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d76b04a4-e324-47e7-833b-0af951ef60bc"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("4bc9e203-7eb6-4bcc-9d35-6637341a9ac0"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("cbfeff57-54b6-4978-bcb1-74ac4fd44acc"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"),
                column: "ConcurrencyStamp",
                value: "143e7e7c-9e63-4317-8ca1-447028a132a8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("05186331-8cf3-4368-b5ec-82f7f8c7be30"), "415906f6-22e8-4a81-a806-995004af3b29", "Teacher", "TEACHER" },
                    { new Guid("68aebce1-de96-4bfa-8ec5-b74a9305991f"), "fe586d32-a01a-4bdb-bbe7-1f2c4d8d3188", "Guardian", "GUARDIAN" },
                    { new Guid("f96134f7-4cc8-498f-be62-28c4634cd90a"), "d2562752-92bc-4049-9b6d-47400f3546fa", "Student", "STUDENT" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEAYSotWbJgn3pJz0rGKOt4BtDHzL6//HHquTRYVFXYQx+pzbAqnHGnin3JX5cj1ktA==");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("2a3711a1-8ffa-463d-aee0-915980c5f7ff"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("66c7de80-aa2b-492d-b6d2-619d5b605f5e"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_RoleId",
                table: "AspNetUsers",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetRoles_RoleId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("05186331-8cf3-4368-b5ec-82f7f8c7be30"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("68aebce1-de96-4bfa-8ec5-b74a9305991f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f96134f7-4cc8-498f-be62-28c4634cd90a"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("2a3711a1-8ffa-463d-aee0-915980c5f7ff"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("66c7de80-aa2b-492d-b6d2-619d5b605f5e"));

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"),
                column: "ConcurrencyStamp",
                value: "444924a1-8ad1-4dc9-b076-34650a42d294");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("a0638004-c5e4-4225-8a7d-7788e33e9629"), "5431d171-5629-4752-9422-ae11a1be9f2f", "Teacher", "TEACHER" },
                    { new Guid("c01c17e1-0f32-40ca-a467-4379cde5bc98"), "620e7a98-e56d-408c-abcf-d6e13d3dfe9b", "Guardian", "GUARDIAN" },
                    { new Guid("d76b04a4-e324-47e7-833b-0af951ef60bc"), "d9b1770b-0580-489a-be94-ea2ba312ba47", "Student", "STUDENT" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEBSPZSqYhOBgFq/HhEvApq+gJDa/LNpTB+mOIcR2z1sS0YWEO2ldKRCnFrDUUh4I9w==");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("4bc9e203-7eb6-4bcc-9d35-6637341a9ac0"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("cbfeff57-54b6-4978-bcb1-74ac4fd44acc"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });
        }
    }
}
