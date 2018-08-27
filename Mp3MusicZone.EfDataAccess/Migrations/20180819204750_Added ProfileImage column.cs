using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mp3MusicZone.EfDataAccess.Migrations
{
    public partial class AddedProfileImagecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImage",
                table: "Users",
                maxLength: 5242880,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImage",
                table: "Users");
        }
    }
}
