using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KyberKlass.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: static table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_AspNetRoles", static x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "Schools",
                columns: static table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_Schools", static x => x.Id);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: static table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_AspNetRoleClaims", static x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: static x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: static table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "DATE", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUsers", static x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AspNetUsers_AspNetRoles_RoleId",
                        column: static x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: static table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUserClaims", static x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: static x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: static table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUserLogins", static x => new { x.LoginProvider, x.ProviderKey });
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: static x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: static table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUserRoles", static x => new { x.UserId, x.RoleId });
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: static x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: static x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: static table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_AspNetUserTokens", static x => new { x.UserId, x.LoginProvider, x.Name });
                    _ = table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: static x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Guardians",
                columns: static table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_Guardians", static x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Guardians_AspNetUsers_Id",
                        column: static x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Teachers",
                columns: static table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_Teachers", static x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Teachers_AspNetUsers_Id",
                        column: static x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Classrooms",
                columns: static table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    TeacherId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_Classrooms", static x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Classrooms_Schools_SchoolId",
                        column: static x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Classrooms_Teachers_TeacherId",
                        column: static x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            _ = migrationBuilder.CreateTable(
                name: "Students",
                columns: static table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuardianId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SchoolId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassroomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: static table =>
                {
                    _ = table.PrimaryKey("PK_Students", static x => x.Id);
                    _ = table.ForeignKey(
                        name: "FK_Students_AspNetUsers_Id",
                        column: static x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    _ = table.ForeignKey(
                        name: "FK_Students_Classrooms_ClassroomId",
                        column: static x => x.ClassroomId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "FK_Students_Guardians_GuardianId",
                        column: static x => x.GuardianId,
                        principalTable: "Guardians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    _ = table.ForeignKey(
                        name: "FK_Students_Schools_SchoolId",
                        column: static x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            _ = migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("030e5da8-3353-47a2-9177-9b21ef1e6153"), "169716f1-c758-4f0e-9042-55f1ef72558a", "Teacher", "TEACHER" },
                    { new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"), "8dd26cf5-b335-4e86-bb63-288c9b6b87fa", "Admin", "ADMIN" },
                    { new Guid("7eec8f19-a7a5-488d-9539-9e4378e10342"), "66d91f5e-3ba1-4bad-9074-0461b8c40036", "Guardian", "GUARDIAN" },
                    { new Guid("f167e1a7-8816-45d2-9379-185b1f32c1fd"), "61a77a0e-c24f-41e7-b1a4-58ba216144aa", "Student", "STUDENT" }
                });

            _ = migrationBuilder.InsertData(
                table: "Schools",
                columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                values: new object[,]
                {
                    { new Guid("77b85c9a-7e44-464c-9505-a76bef835376"), "Rnd. Address", "test_schoolc@ez.com", true, "Test School", "02 987 0000" },
                    { new Guid("7a9183f1-5805-4cba-bf48-95200ca103dd"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" },
                    { new Guid("80c02b02-1329-4af5-abb7-ebd99f46641a"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" }
                });

            _ = migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "BirthDate", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "RoleId", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"), 0, "1416 Ryan Mountains", new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "d4f6406d-9b51-4290-994d-cf1bb9668b5e", "admin@kyberklass.com", false, "Admin", true, "User", false, null, "ADMIN@KYBERKLASS.COM", "ADMIN@KYBERKLASS.COM", "AQAAAAEAACcQAAAAED7GipdqIG2zZQk4tE1T+4t0FokOqU7K/3J7k6zmCOLWHjrv+p0Onu/FzwVjAieVQQ==", "08888888888", false, new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"), "0DB9D047-3375-4739-9C32-217CC8337032", false, "admin@kyberklass.com" });

            _ = migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("420abb62-30a5-4983-835e-fe0a46b6f463"), new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98") });

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            _ = migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            _ = migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            _ = migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoleId",
                table: "AspNetUsers",
                column: "RoleId");

            _ = migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Classrooms_SchoolId",
                table: "Classrooms",
                column: "SchoolId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Classrooms_TeacherId",
                table: "Classrooms",
                column: "TeacherId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Students_ClassroomId",
                table: "Students",
                column: "ClassroomId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Students_GuardianId",
                table: "Students",
                column: "GuardianId");

            _ = migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            _ = migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            _ = migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            _ = migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            _ = migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            _ = migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            _ = migrationBuilder.DropTable(
                name: "Students");

            _ = migrationBuilder.DropTable(
                name: "Classrooms");

            _ = migrationBuilder.DropTable(
                name: "Guardians");

            _ = migrationBuilder.DropTable(
                name: "Schools");

            _ = migrationBuilder.DropTable(
                name: "Teachers");

            _ = migrationBuilder.DropTable(
                name: "AspNetUsers");

            _ = migrationBuilder.DropTable(
                name: "AspNetRoles");
        }
    }
}