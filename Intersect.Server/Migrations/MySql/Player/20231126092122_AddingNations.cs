using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.MySql.Player
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
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "NationJoinDate",
                table: "Players",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Nations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FoundingDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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