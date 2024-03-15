    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    #nullable disable

    namespace KyberKlass.Data.Migrations
    {
        public partial class AddUserIdToGuardian : Migration
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

                migrationBuilder.AddColumn<Guid>(
                    name: "UserId",
                    table: "Guardians",
                    type: "uniqueidentifier",
                    nullable: false,
                    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

                migrationBuilder.UpdateData(
                    table: "AspNetUsers",
                    keyColumn: "Id",
                    keyValue: new Guid("e321fa43-9c90-4e01-8f0a-002eae899e98"),
                    column: "PasswordHash",
                    value: "AQAAAAEAACcQAAAAENobf8Ua5h68uU2W3m6iVGKp9Ep2gftS/UwFECzUnEM0jlslzIb/xanbD0g2/9goLA==");

                migrationBuilder.InsertData(
                    table: "Schools",
                    columns: new[] { "Id", "Address", "Email", "IsActive", "Name", "PhoneNumber" },
                    values: new object[,]
                    {
                        { new Guid("899abd80-f1ab-4e4c-b796-87f9536ee405"), "Sofia Center, Pozitano St 26, 1000 Sofia", "schoolb@ez.com", true, "91. Немска езикова гимназия „Проф. Константин Гълъбов“", "02 987 5305" },
                        { new Guid("c2e6373c-08c3-4582-a436-d96ed501c920"), "Promishlena zona Hladilnika, bul. \"Nikola Y. Vaptsarov\" 47, 1407 Sofia", "st@example.com", true, "St. George International School", "02 414 4414" }
                    });

                migrationBuilder.CreateIndex(
                    name: "IX_Guardians_UserId",
                    table: "Guardians",
                    column: "UserId");

                migrationBuilder.AddForeignKey(
                    name: "FK_Guardians_AspNetUsers_UserId",
                    table: "Guardians",
                    column: "UserId",
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            }

            protected override void Down(MigrationBuilder migrationBuilder)
            {
                migrationBuilder.DropForeignKey(
                    name: "FK_Guardians_AspNetUsers_UserId",
                    table: "Guardians");

                migrationBuilder.DropIndex(
                    name: "IX_Guardians_UserId",
                    table: "Guardians");

                migrationBuilder.DeleteData(
                    table: "Schools",
                    keyColumn: "Id",
                    keyValue: new Guid("899abd80-f1ab-4e4c-b796-87f9536ee405"));

                migrationBuilder.DeleteData(
                    table: "Schools",
                    keyColumn: "Id",
                    keyValue: new Guid("c2e6373c-08c3-4582-a436-d96ed501c920"));

                migrationBuilder.DropColumn(
                    name: "IsActive",
                    table: "Guardians");

                migrationBuilder.DropColumn(
                    name: "UserId",
                    table: "Guardians");

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
            }
        }
    }
