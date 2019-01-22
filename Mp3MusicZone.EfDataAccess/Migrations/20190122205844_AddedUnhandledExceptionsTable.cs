using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mp3MusicZone.EfDataAccess.Migrations
{
    public partial class AddedUnhandledExceptionsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UnhandledExceptionEntries",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ExceptionMessage = table.Column<string>(nullable: true),
                    ExceptionType = table.Column<string>(nullable: true),
                    AdditionalInfo = table.Column<string>(nullable: true),
                    TimeOfExecution = table.Column<DateTime>(nullable: false),
                    StackTrace = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnhandledExceptionEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UnhandledExceptionEntries");
        }
    }
}
