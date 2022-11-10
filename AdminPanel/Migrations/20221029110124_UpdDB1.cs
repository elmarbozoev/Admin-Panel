using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AdminPanel.Migrations
{
    public partial class UpdDB1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Surname",
                table: "Employees");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employees",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Employees",
                keyColumn: "Name",
                keyValue: null,
                column: "Name",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Employees",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                table: "Employees",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
