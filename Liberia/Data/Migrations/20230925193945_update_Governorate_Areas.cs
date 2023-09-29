using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liberia.Data.Migrations
{
    public partial class update_Governorate_Areas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Governorates",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Areas",
                newName: "NameEn");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Governorates",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Areas",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Governorates");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Areas");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Governorates",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "Areas",
                newName: "Name");
        }
    }
}
