using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class UpdateBirthDateDataType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("2b103025-80f5-45f7-bda2-3cba8a65d010"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("51d0a363-e07c-470d-8ca5-6cfc58999f24"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cc05fbc7-a33c-4a41-8bfb-201f52306d1a"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("025288d8-905a-43ac-a865-1a9c8724b071"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("3de9fcb3-5b92-492d-82eb-39161a59e5bb"));

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"),
                column: "ConcurrencyStamp",
                value: "88cd068a-cd70-4573-9cf6-38bd2778561d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("01199dd5-c936-4fb0-ab15-6f9e3f55f141"), "a82289dc-41c7-4746-82a0-cac167d89c07", "Teacher", "TEACHER" },
                    { new Guid("24874bc1-7d2b-49d3-945f-932dd2000265"), "0fc7f138-1eae-432e-b8d2-81d4268038f2", "Student", "STUDENT" },
                    { new Guid("87aae012-242b-42e0-9a8c-6730bc4c5f85"), "e0244a79-a3dd-45ea-a59d-329e5182a2f5", "Guardian", "GUARDIAN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                columns: new[] { "BirthDate", "PasswordHash" },
                values: new object[] { new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AQAAAAEAACcQAAAAEG+kJUO7Ak/59UWCzi445OpTJAVrxC2P16io0XoXji1sYBbW4EIKNTOookuJN3LHNw==" });

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("6c405a2e-a4c1-45c9-8bc8-6bae5a3540a3"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" },
                    { new Guid("8b1eee83-2c11-4750-b8cb-968f092cb420"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("01199dd5-c936-4fb0-ab15-6f9e3f55f141"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("24874bc1-7d2b-49d3-945f-932dd2000265"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("87aae012-242b-42e0-9a8c-6730bc4c5f85"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("6c405a2e-a4c1-45c9-8bc8-6bae5a3540a3"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("8b1eee83-2c11-4750-b8cb-968f092cb420"));

            migrationBuilder.AlterColumn<string>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"),
                column: "ConcurrencyStamp",
                value: "9f25f343-c67b-4723-a409-3f336acd46d5");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("2b103025-80f5-45f7-bda2-3cba8a65d010"), "7105e2f9-f315-4e28-aa32-6c36d9ab8346", "Student", "STUDENT" },
                    { new Guid("51d0a363-e07c-470d-8ca5-6cfc58999f24"), "ea87b792-bbb3-4733-bd3b-fdd9706a81cc", "Teacher", "TEACHER" },
                    { new Guid("cc05fbc7-a33c-4a41-8bfb-201f52306d1a"), "4cd83da7-c6cb-4168-9096-ebd754a3ac90", "Guardian", "GUARDIAN" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                columns: new[] { "BirthDate", "PasswordHash" },
                values: new object[] { "01-01-2001", "AQAAAAEAACcQAAAAEPGoHe/UpNo9uocU6MxgVXw/MPawS9EBtCUK9v4YNX0V08lqt8LZWPMFp7h/tO6l9w==" });

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("025288d8-905a-43ac-a865-1a9c8724b071"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("3de9fcb3-5b92-492d-82eb-39161a59e5bb"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });
        }
    }
}
