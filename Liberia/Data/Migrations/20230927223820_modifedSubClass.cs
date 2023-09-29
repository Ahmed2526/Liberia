using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liberia.Data.Migrations
{
    public partial class modifedSubClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "ModifiedOn",
                table: "Subscribers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Subscribers",
                newName: "SubscriberId");

            migrationBuilder.AlterColumn<string>(
                name: "Thumbnail",
                table: "Subscribers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Subscribers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubscriberId",
                table: "Subscribers",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Thumbnail",
                table: "Subscribers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Subscribers",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

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
        }
    }
}
