using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.Sqlite.Player
{
    /// <inheritdoc />
    public partial class AddedGuildPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GuildPoints",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GuildUpgradesData",
                table: "Guilds",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SpentGuildPoints",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GuildPoints",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "GuildUpgradesData",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "SpentGuildPoints",
                table: "Guilds");
        }
    }
}
