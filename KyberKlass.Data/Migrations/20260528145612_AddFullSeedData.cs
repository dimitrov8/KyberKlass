using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace KyberKlass.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFullSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("030e5da8-3353-47a2-9177-9b21ef1e6153"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7eec8f19-a7a5-488d-9539-9e4378e10342"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f167e1a7-8816-45d2-9379-185b1f32c1fd"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("77b85c9a-7e44-464c-9505-a76bef835376"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("7a9183f1-5805-4cba-bf48-95200ca103dd"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("80c02b02-1329-4af5-abb7-ebd99f46641a"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"),
                column: "ConcurrencyStamp",
                value: "dc7b742e-e5e3-4537-af41-32803244d217");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3f262f2b-bd05-4b24-9a6c-bca365d372fb"), "c6d865bf-6f64-4d47-b49f-6866b58619b7", "Teacher", "TEACHER" },
                    { new Guid("74efc216-b31e-4ed3-96d0-7dc2296eec3b"), "8e906f0e-342d-46de-8249-118201686478", "Guardian", "GUARDIAN" },
                    { new Guid("f697e34c-843e-467a-b11f-037e185c180f"), "6cf6c247-d253-4793-a753-cb2eb090b234", "Student", "STUDENT" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                column: "PasswordHash",
                value: "AQAAAAIAAYagAAAAEC5tNYhoMOyMvOrciUf9NiZXU+KjuRqcsUpSfjzBFVcxs4dbf/VuygK600x/tUpYVQ==");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("08ed558b-d9ee-4663-b2b7-e700fd3620e6"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                    { new Guid("1e633a7a-7d6c-413a-b8bf-3081324cee7e"), "Rnd. Address", "test_schoolc@ez.com", true, "Test School", "02 987 0000" },
                    { new Guid("6cead59f-b8d4-4528-bd6d-e0e5dc97eaee"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RoleId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("427a6f9a-bee1-4493-9d13-65ab478bb5f5"), 0, "15 Teacher Lane", new DateTime(1999, 8, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "0f9d3f0d-9f65-4f72-9a3f-3f95cb98f8f1", "teacher@kyberklass.com", false, "Alice", true, "Johnson", false, null, "TEACHER@KYBERKLASS.COM", "TEACHER@KYBERKLASS.COM", "AQAAAAIAAYagAAAAEBMhCEHC1SlQhTUbcVXS+qy1gIGblfslsv9YtBPjZjBmWFpv2r+m1gQ7eifAWnQmsg==", "0881111111", false, new Guid("3f262f2b-bd05-4b24-9a6c-bca365d372fb"), "A1B2C3D4-E5F6-7890-ABCD-EF1234567890", false, "teacher@kyberklass.com" },
                    { new Guid("58d10548-8f58-48ba-9eff-73e0d87bf7b9"), 0, "4445 Smithfield Avenue", new DateTime(2011, 3, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "b7d2e4f9-1c88-4a10-93f6-7e2c5d1a9b33", "student2@kyberklass.com", false, "Crystal", true, "Lane", false, null, "STUDENT2@KYBERKLASS.COM", "STUDENT2@KYBERKLASS.COM", "AQAAAAIAAYagAAAAEATrWWc4wiAkLGWlHp+hz/Afyw/vl7ykXjDoVQgic0wJFIjvRS4ERbnpLHehJRIi5A==", "0887654321", false, new Guid("f697e34c-843e-467a-b11f-037e185c180f"), "3E9B1C57-8A44-42F0-9D61-5C7A2E8F0B19", false, "student2@kyberklass.com" },
                    { new Guid("9dae1d79-f917-4acd-bc32-b78408e702a3"), 0, "16 Teacher Blvd", new DateTime(1998, 9, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "2c6e4b80-1b6a-4f1d-a83d-4bfbda34f4c7", "teacher2@kyberklass.com", false, "Leon", true, "Leblanc", false, null, "TEACHER2@KYBERKLASS.COM", "TEACHER2@KYBERKLASS.COM", "AQAAAAIAAYagAAAAEEBW0XfusVI/5/B9vVQUeyJ1znh3TzHdMaJYEGuAhEPLGS0tHN/nmsd/Xl4RwyuGsA==", "0882222222", false, new Guid("3f262f2b-bd05-4b24-9a6c-bca365d372fb"), "B2C3D4E5-F6A7-8901-BCDE-F12345678901", false, "teacher2@kyberklass.com" },
                    { new Guid("b0aab2ce-41d6-4a7e-b9d4-0b2c7ced288a"), 0, "4445 Smithfield Avenue", new DateTime(2010, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "4f8c1d7a-9b22-4d66-a5e1-2c7f9b0d3e55", "student@kyberklass.com", false, "Neo", true, "Joyce", false, null, "STUDENT@KYBERKLASS.COM", "STUDENT@KYBERKLASS.COM", "AQAAAAIAAYagAAAAEPVO2VHn1sUvzMAWmdmsv5Q3TFcqhuQUs7VWkALHyG6ODp1VEripi+tOEJhhsaxybw==", "0881234567", false, new Guid("f697e34c-843e-467a-b11f-037e185c180f"), "A7D2F941-5C88-4E19-B3D0-6F2A8C7E1B44", false, "student@kyberklass.com" },
                    { new Guid("db0dc8be-db06-4b53-a0e3-1a5cb3fa40da"), 0, "4445 Smithfield Avenue", new DateTime(2002, 2, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "d4f6406d-9b51-4290-994d-cf1bb9668b5e", "guardian@kyberklass.com", false, "Belle", true, "Moon", false, null, "GUARDIAN@KYBERKLASS.COM", "GUARDIAN@KYBERKLASS.COM", "AQAAAAIAAYagAAAAEHYICPBp9IFFJqFmVQJHIB4siaTffkQp8qDPSH0Y56+C9fBswnIyq4XMxtG/KA8K/w==", "08844444444", false, new Guid("74efc216-b31e-4ed3-96d0-7dc2296eec3b"), "0DB9D047-3375-4739-9C32-217CC8337032", false, "guardian@kyberklass.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("3f262f2b-bd05-4b24-9a6c-bca365d372fb"), new Guid("427a6f9a-bee1-4493-9d13-65ab478bb5f5") },
                    { new Guid("f697e34c-843e-467a-b11f-037e185c180f"), new Guid("58d10548-8f58-48ba-9eff-73e0d87bf7b9") },
                    { new Guid("3f262f2b-bd05-4b24-9a6c-bca365d372fb"), new Guid("9dae1d79-f917-4acd-bc32-b78408e702a3") },
                    { new Guid("f697e34c-843e-467a-b11f-037e185c180f"), new Guid("b0aab2ce-41d6-4a7e-b9d4-0b2c7ced288a") },
                    { new Guid("74efc216-b31e-4ed3-96d0-7dc2296eec3b"), new Guid("db0dc8be-db06-4b53-a0e3-1a5cb3fa40da") }
                });

            migrationBuilder.InsertData(
                table: "Guardians",
                column: "Id",
                value: new Guid("db0dc8be-db06-4b53-a0e3-1a5cb3fa40da"));

            migrationBuilder.InsertData(
                table: "Teachers",
                column: "Id",
                values: new object[]
                {
                    new Guid("427a6f9a-bee1-4493-9d13-65ab478bb5f5"),
                    new Guid("9dae1d79-f917-4acd-bc32-b78408e702a3")
                });

            migrationBuilder.InsertData(
                table: "Classrooms",
                columns: new[] { "Id", "IsActive", "Name", "SchoolId", "TeacherId" },
                values: new object[,]
                {
                    { new Guid("3b9a4da2-8b83-4e71-ab43-916663ca9ca4"), true, "A", new Guid("6cead59f-b8d4-4528-bd6d-e0e5dc97eaee"), new Guid("427a6f9a-bee1-4493-9d13-65ab478bb5f5") },
                    { new Guid("63e35421-bd57-4de6-940d-e239c1886ad5"), true, "B", new Guid("08ed558b-d9ee-4663-b2b7-e700fd3620e6"), new Guid("9dae1d79-f917-4acd-bc32-b78408e702a3") }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "ClassroomId", "GuardianId", "SchoolId" },
                values: new object[,]
                {
                    { new Guid("58d10548-8f58-48ba-9eff-73e0d87bf7b9"), new Guid("63e35421-bd57-4de6-940d-e239c1886ad5"), new Guid("db0dc8be-db06-4b53-a0e3-1a5cb3fa40da"), new Guid("08ed558b-d9ee-4663-b2b7-e700fd3620e6") },
                    { new Guid("b0aab2ce-41d6-4a7e-b9d4-0b2c7ced288a"), new Guid("3b9a4da2-8b83-4e71-ab43-916663ca9ca4"), new Guid("db0dc8be-db06-4b53-a0e3-1a5cb3fa40da"), new Guid("6cead59f-b8d4-4528-bd6d-e0e5dc97eaee") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("3f262f2b-bd05-4b24-9a6c-bca365d372fb"), new Guid("427a6f9a-bee1-4493-9d13-65ab478bb5f5") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("f697e34c-843e-467a-b11f-037e185c180f"), new Guid("58d10548-8f58-48ba-9eff-73e0d87bf7b9") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("3f262f2b-bd05-4b24-9a6c-bca365d372fb"), new Guid("9dae1d79-f917-4acd-bc32-b78408e702a3") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("f697e34c-843e-467a-b11f-037e185c180f"), new Guid("b0aab2ce-41d6-4a7e-b9d4-0b2c7ced288a") });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("74efc216-b31e-4ed3-96d0-7dc2296eec3b"), new Guid("db0dc8be-db06-4b53-a0e3-1a5cb3fa40da") });

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("1e633a7a-7d6c-413a-b8bf-3081324cee7e"));

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("58d10548-8f58-48ba-9eff-73e0d87bf7b9"));

            migrationBuilder.DeleteData(
                table: "Students",
                keyColumn: "Id",
                keyValue: new Guid("b0aab2ce-41d6-4a7e-b9d4-0b2c7ced288a"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("58d10548-8f58-48ba-9eff-73e0d87bf7b9"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("b0aab2ce-41d6-4a7e-b9d4-0b2c7ced288a"));

            migrationBuilder.DeleteData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: new Guid("3b9a4da2-8b83-4e71-ab43-916663ca9ca4"));

            migrationBuilder.DeleteData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: new Guid("63e35421-bd57-4de6-940d-e239c1886ad5"));

            migrationBuilder.DeleteData(
                table: "Guardians",
                keyColumn: "Id",
                keyValue: new Guid("db0dc8be-db06-4b53-a0e3-1a5cb3fa40da"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("f697e34c-843e-467a-b11f-037e185c180f"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("db0dc8be-db06-4b53-a0e3-1a5cb3fa40da"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("08ed558b-d9ee-4663-b2b7-e700fd3620e6"));

            migrationBuilder.DeleteData(
                table: "Schools",
                keyColumn: "Id",
                keyValue: new Guid("6cead59f-b8d4-4528-bd6d-e0e5dc97eaee"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: new Guid("427a6f9a-bee1-4493-9d13-65ab478bb5f5"));

            migrationBuilder.DeleteData(
                table: "Teachers",
                keyColumn: "Id",
                keyValue: new Guid("9dae1d79-f917-4acd-bc32-b78408e702a3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("74efc216-b31e-4ed3-96d0-7dc2296eec3b"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("427a6f9a-bee1-4493-9d13-65ab478bb5f5"));

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("9dae1d79-f917-4acd-bc32-b78408e702a3"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("3f262f2b-bd05-4b24-9a6c-bca365d372fb"));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"),
                column: "ConcurrencyStamp",
                value: "8dd26cf5-b335-4e86-bb63-288c9b6b87fa");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("030e5da8-3353-47a2-9177-9b21ef1e6153"), "169716f1-c758-4f0e-9042-55f1ef72558a", "Teacher", "TEACHER" },
                    { new Guid("7eec8f19-a7a5-488d-9539-9e4378e10342"), "66d91f5e-3ba1-4bad-9074-0461b8c40036", "Guardian", "GUARDIAN" },
                    { new Guid("f167e1a7-8816-45d2-9379-185b1f32c1fd"), "61a77a0e-c24f-41e7-b1a4-58ba216144aa", "Student", "STUDENT" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                column: "PasswordHash",
                value: "AQAAAAEAACcQAAAAED7GipdqIG2zZQk4tE1T+4t0FokOqU7K/3J7k6zmCOLWHjrv+p0Onu/FzwVjAieVQQ==");

            migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("77b85c9a-7e44-464c-9505-a76bef835376"), "Rnd. Address", "test_schoolc@ez.com", true, "Test School", "02 987 0000" },
                    { new Guid("7a9183f1-5805-4cba-bf48-95200ca103dd"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" },
                    { new Guid("80c02b02-1329-4af5-abb7-ebd99f46641a"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" }
                });
        }
    }
}
