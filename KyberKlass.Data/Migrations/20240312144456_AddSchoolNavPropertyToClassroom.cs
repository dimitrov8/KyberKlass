using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class AddSchoolNavPropertyToClassroom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Schools_SchoolId",
                table: "Classrooms");

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("2a3711a1-8ffa-463d-aee0-915980c5f7ff"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("66c7de80-aa2b-492d-b6d2-619d5b605f5e"));

            migrationBuilder.RenameColumn(
                name: "IsWorking",
                table: "Teachers",
                newName: "IsActive");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                table: "Classrooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("47895c27-0ae0-4b02-850d-a0e2d83fead3"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" },
                    { new Guid("5cb7619e-5352-4afa-81f7-e0ce19996506"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Schools_SchoolId",
                table: "Classrooms",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Schools_SchoolId",
                table: "Classrooms");

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("47895c27-0ae0-4b02-850d-a0e2d83fead3"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("5cb7619e-5352-4afa-81f7-e0ce19996506"));

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Teachers",
                newName: "IsWorking");

            migrationBuilder.AlterColumn<Guid>(
                name: "SchoolId",
                table: "Classrooms",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("2a3711a1-8ffa-463d-aee0-915980c5f7ff"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("66c7de80-aa2b-492d-b6d2-619d5b605f5e"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Schools_SchoolId",
                table: "Classrooms",
                column: "SchoolId",
                principalTable: "Schools",
                principalColumn: "Id");
        }
    }
}
