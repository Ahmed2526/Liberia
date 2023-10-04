using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liberia.Data.Migrations
{
    public partial class ModifiedBookTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbNail",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbNail",
                table: "Books");
        }
    }
}
