using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class AddIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("806dbcef-1d09-43d0-a0cf-34e1d807a8de"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d3d03709-3739-4d37-bb34-50203e6aaa0f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d499bfe4-60b7-4879-b456-87174d861d1d"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("da54b08d-06a4-44ae-bc4c-d67f7867f398"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("e25b1707-bba4-49da-918b-80c465fcbfa9"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

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
                columns: new[] { "IsActive", "PasswordHash" },
                values: new object[] { true, "AQAAAAEAACcQAAAAEPGoHe/UpNo9uocU6MxgVXw/MPawS9EBtCUK9v4YNX0V08lqt8LZWPMFp7h/tO6l9w==" });

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("025288d8-905a-43ac-a865-1a9c8724b071"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("3de9fcb3-5b92-492d-82eb-39161a59e5bb"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"),
                column: "ConcurrencyStamp",
                value: "b7291292-e39e-4aaa-9df1-b42ee576ee45");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("806dbcef-1d09-43d0-a0cf-34e1d807a8de"), "d8219396-78b9-452a-8c79-d8afaedd159c", "Guardian", "GUARDIAN" },
                    { new Guid("d3d03709-3739-4d37-bb34-50203e6aaa0f"), "eed63e22-d964-42da-8e56-19cb38e3d275", "Teacher", "TEACHER" },
                    { new Guid("d499bfe4-60b7-4879-b456-87174d861d1d"), "2889ab55-5f89-4196-9cb1-9693c4d412aa", "Student", "STUDENT" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEJdvYTQDkzW4Rbgn7e5htaVdsjy7r+IHircTV0acCoReGDNOGsS/6TMnDHCghGtyQA==");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("da54b08d-06a4-44ae-bc4c-d67f7867f398"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" },
                    { new Guid("e25b1707-bba4-49da-918b-80c465fcbfa9"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" }
                });
        }
    }
}
