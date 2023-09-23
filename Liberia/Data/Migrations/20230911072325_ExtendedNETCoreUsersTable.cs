﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Liberia.Data.Migrations
{
	public partial class ExtendedNETCoreUsersTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<DateTime>(
				name: "CreatedOn",
				table: "AspNetUsers",
				type: "datetime2",
				nullable: false,
				defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

			migrationBuilder.AddColumn<string>(
				name: "FullName",
				table: "AspNetUsers",
				type: "nvarchar(150)",
				maxLength: 150,
				nullable: false,
				defaultValue: "");

			migrationBuilder.AddColumn<bool>(
				name: "IsActive",
				table: "AspNetUsers",
				type: "bit",
				nullable: false,
				defaultValue: false);

			migrationBuilder.AddColumn<DateTime>(
				name: "ModifiedOn",
				table: "AspNetUsers",
				type: "datetime2",
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "CreatedOn",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "FullName",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "IsActive",
				table: "AspNetUsers");

			migrationBuilder.DropColumn(
				name: "ModifiedOn",
				table: "AspNetUsers");
		}
	}
}
