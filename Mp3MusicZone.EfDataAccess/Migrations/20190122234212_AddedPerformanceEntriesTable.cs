using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mp3MusicZone.EfDataAccess.Migrations
{
    public partial class AddedPerformanceEntriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PerformanceEntries",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    TimeOfExecution = table.Column<DateTime>(nullable: false),
                    Operation = table.Column<string>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    OperationData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerformanceEntries", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PerformanceEntries");
        }
    }
}
