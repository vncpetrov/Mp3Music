using Microsoft.EntityFrameworkCore.Migrations;

namespace Mp3MusicZone.EfDataAccess.Migrations
{
    public partial class UrlColumnRenamedToAdditionalInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "UnhandledExceptionEntries",
                newName: "AdditionalInfo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdditionalInfo",
                table: "UnhandledExceptionEntries",
                newName: "Url");
        }
    }
}
