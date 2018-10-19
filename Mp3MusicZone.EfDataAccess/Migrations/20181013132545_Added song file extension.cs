using Microsoft.EntityFrameworkCore.Migrations;

namespace Mp3MusicZone.EfDataAccess.Migrations
{
    public partial class Addedsongfileextension : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Songs",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Songs");
        }
    }
}
