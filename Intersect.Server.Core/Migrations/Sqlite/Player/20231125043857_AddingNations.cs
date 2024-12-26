using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.Sqlite.Player
{
    /// <inheritdoc />
    public partial class AddingNations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DbNationId",
                table: "Players",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "NationJoinDate",
                table: "Players",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Nations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    FoundingDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Players_DbNationId",
                table: "Players",
                column: "DbNationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Nations_DbNationId",
                table: "Players",
                column: "DbNationId",
                principalTable: "Nations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Nations_DbNationId",
                table: "Players");

            migrationBuilder.DropTable(
                name: "Nations");

            migrationBuilder.DropIndex(
                name: "IX_Players_DbNationId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "DbNationId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "NationJoinDate",
                table: "Players");
        }
    }
}