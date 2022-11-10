using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Migrations
{
    public partial class PicturePathUpd1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "Pictures",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "Pictures");
        }
    }
}
