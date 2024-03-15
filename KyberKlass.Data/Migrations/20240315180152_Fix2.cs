using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class Fix2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Guardian_GuardianId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guardian",
                table: "Guardian");

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("1da88108-5fa2-475f-9345-01f1215e7a76"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("41b738cb-7067-4ac7-ad86-6e802ecae38b"));

            migrationBuilder.RenameTable(
                name: "Guardian",
                newName: "Guardians");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guardians",
                table: "Guardians",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAELzJ4L6dHQtFa8SSmfWGeBkGjGH5KvR10g/Nt+qXhc30oSllSF0k9jKLvbffQPstvQ==");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("5424423b-2107-4868-a0a3-d075054707f3"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("8987040d-4e11-4ea0-996c-0b6479945378"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Guardians_GuardianId",
                table: "Students",
                column: "GuardianId",
                principalTable: "Guardians",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Guardians_GuardianId",
                table: "Students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guardians",
                table: "Guardians");

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("5424423b-2107-4868-a0a3-d075054707f3"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("8987040d-4e11-4ea0-996c-0b6479945378"));

            migrationBuilder.RenameTable(
                name: "Guardians",
                newName: "Guardian");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guardian",
                table: "Guardian",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Guardian_GuardianId",
                table: "Students",
                column: "GuardianId",
                principalTable: "Guardian",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
