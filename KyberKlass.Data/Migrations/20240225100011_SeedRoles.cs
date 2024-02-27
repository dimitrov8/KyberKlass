using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class SeedRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("20147dbd-1528-4032-b7e1-564aa94a4b6f"), "ea37ffac-9af4-4347-ac48-f47b3fe1b0ee", "Teacher", "TEACHER" },
                    { new Guid("5c9c3682-f6ec-4eee-8e25-5d65867a1ca6"), "46557ae2-306d-4a11-a026-846779786045", "Admin", "ADMIN" },
                    { new Guid("6bf41d91-06c0-468d-adf5-b13a1e006145"), "76700360-da5c-43b9-8dd4-6b3e37e6d187", "Student", "STUDENT" },
                    { new Guid("f954d71e-73db-4dbb-81a3-ff3a4a928af4"), "e039ebd6-ea4a-4aa3-b6f3-a582f6312159", "Guardian", "GUARDIAN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("20147dbd-1528-4032-b7e1-564aa94a4b6f"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("5c9c3682-f6ec-4eee-8e25-5d65867a1ca6"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("6bf41d91-06c0-468d-adf5-b13a1e006145"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f954d71e-73db-4dbb-81a3-ff3a4a928af4"));
        }
    }
}
