using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liberia.Data.Migrations
{
    public partial class modifedSubClassv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "SubscriberId",
                table: "Subscribers");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Subscribers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Subscribers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Subscribers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedOn",
                table: "Subscribers",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Subscribers");

            migrationBuilder.AddColumn<int>(
                name: "SubscriberId",
                table: "Subscribers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Subscribers",
                table: "Subscribers",
                column: "SubscriberId");
        }
    }
}
