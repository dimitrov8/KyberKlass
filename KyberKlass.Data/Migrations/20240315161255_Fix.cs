using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class Fix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("47895c27-0ae0-4b02-850d-a0e2d83fead3"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("5cb7619e-5352-4afa-81f7-e0ce19996506"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEMO7zPBd9ZP7NS0XEwA7zkUQO/aGW5R51VvQIWJZWFymwfhcqFwRiqSZdpoO3WKwpw==");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("1da88108-5fa2-475f-9345-01f1215e7a76"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("41b738cb-7067-4ac7-ad86-6e802ecae38b"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("1da88108-5fa2-475f-9345-01f1215e7a76"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("41b738cb-7067-4ac7-ad86-6e802ecae38b"));

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEBlmdB0LkZU4eo20caNs50COjIozKcjATtj6XvFz9uNZZGFqLMaaj3uhXb75yn4uOA==");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("47895c27-0ae0-4b02-850d-a0e2d83fead3"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" },
                    { new Guid("5cb7619e-5352-4afa-81f7-e0ce19996506"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" }
                });
        }
    }
}
