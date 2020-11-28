using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persistence.Migrations
{
    public partial class CreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lists",
                columns: table => new
                {
                    _id = table.Column<Guid>(nullable: false),
                    User = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lists", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ListItem",
                columns: table => new
                {
                    _id = table.Column<Guid>(nullable: false),
                    Desc = table.Column<string>(nullable: false),
                    Url = table.Column<string>(nullable: true),
                    List_id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListItem", x => x._id);
                    table.ForeignKey(
                        name: "FK_ListItem_Lists_List_id",
                        column: x => x.List_id,
                        principalTable: "Lists",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { new Guid("8ef3044c-7254-414c-8e79-9971d672091b"), "Barry@gmail.com", "Barry", "newpassword" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name", "Password" },
                values: new object[] { new Guid("dd93ffaa-97df-44e3-81e3-5caf8c42abc2"), "roboto@gmail.com", "Mr. Roboto", "youllneverguessthis" });

            migrationBuilder.CreateIndex(
                name: "IX_ListItem_List_id",
                table: "ListItem",
                column: "List_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ListItem");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Lists");
        }
    }
}
