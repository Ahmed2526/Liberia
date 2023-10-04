using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liberia.Data.Migrations
{
    public partial class Modified_Category : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastUpdatedOn",
                table: "Categories",
                newName: "ModifiedOn");

            migrationBuilder.RenameColumn(
                name: "IsDeleted",
                table: "Categories",
                newName: "IsActive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedOn",
                table: "Categories",
                newName: "LastUpdatedOn");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Categories",
                newName: "IsDeleted");
        }
    }
}
