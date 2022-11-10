using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Migrations
{
    public partial class UpdNewsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MainPictureIndex",
                table: "News",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MainPictureIndex",
                table: "News");
        }
    }
}
