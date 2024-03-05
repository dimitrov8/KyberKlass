using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class UpdateUserBirthDateColumnType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "DATE",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDate",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "DATE");

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
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAEG+kJUO7Ak/59UWCzi445OpTJAVrxC2P16io0XoXji1sYBbW4EIKNTOookuJN3LHNw==");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("6c405a2e-a4c1-45c9-8bc8-6bae5a3540a3"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" },
                    { new Guid("8b1eee83-2c11-4750-b8cb-968f092cb420"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" }
                });
        }
    }
}
