using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class Initial_Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 20, nullable: false),
                    Surname = table.Column<string>(nullable: true),
                    Age = table.Column<short>(nullable: false),
                    Address_Street = table.Column<string>(nullable: true),
                    Address_City = table.Column<string>(nullable: true),
                    Address_State = table.Column<string>(nullable: true),
                    Address_Country = table.Column<string>(nullable: true),
                    Address_ZipCode = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    IsCompleted = table.Column<bool>(nullable: false),
                    Priority = table.Column<byte>(nullable: false),
                    State = table.Column<byte>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<long>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    LastModifiedUserId = table.Column<long>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedDate = table.Column<DateTime>(nullable: true),
                    DeletedUserId = table.Column<long>(nullable: true),
                    AssignedPersonId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Persons_AssignedPersonId",
                        column: x => x.AssignedPersonId,
                        principalTable: "Persons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_AssignedPersonId",
                table: "Tasks",
                column: "AssignedPersonId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "Persons");
        }
    }
}
