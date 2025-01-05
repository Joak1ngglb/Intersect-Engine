﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.Sqlite.Game
{
    /// <inheritdoc />
    public partial class AddCraftAndResourceFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ExperienceAmount",
                table: "Resources",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Jobs",
                table: "Resources",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExperienceAmount",
                table: "Crafts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Jobs",
                table: "Crafts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceAmount",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "Jobs",
                table: "Resources");

            migrationBuilder.DropColumn(
                name: "ExperienceAmount",
                table: "Crafts");

            migrationBuilder.DropColumn(
                name: "Jobs",
                table: "Crafts");
        }
    }
}
