using Microsoft.EntityFrameworkCore.Migrations;

namespace Mp3MusicZone.EfDataAccess.Migrations
{
    public partial class ListeningscolumnaddedtoSongs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Listenings",
                table: "Songs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Listenings",
                table: "Songs");
        }
    }
}
