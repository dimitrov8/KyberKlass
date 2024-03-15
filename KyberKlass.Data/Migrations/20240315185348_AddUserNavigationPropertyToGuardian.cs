using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class AddUserNavigationPropertyToGuardian : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("5424423b-2107-4868-a0a3-d075054707f3"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("8987040d-4e11-4ea0-996c-0b6479945378"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Guardians",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("703c3ab0-8dc2-4e3d-b593-ef413e385061"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("8493fab2-3b55-45df-b52f-71024dcc7d35"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Guardians_AspNetUsers_Id",
                table: "Guardians",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Guardians_AspNetUsers_Id",
                table: "Guardians");

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("703c3ab0-8dc2-4e3d-b593-ef413e385061"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("8493fab2-3b55-45df-b52f-71024dcc7d35"));

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Guardians");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("5424423b-2107-4868-a0a3-d075054707f3"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("8987040d-4e11-4ea0-996c-0b6479945378"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });
        }
    }
}
