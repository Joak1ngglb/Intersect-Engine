using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.Sqlite.Player
{
    /// <inheritdoc />
    public partial class AddGuildLogos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "BackgroundB",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "BackgroundG",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "BackgroundR",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "LogoBackground",
                table: "Guilds",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LogoSymbol",
                table: "Guilds",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "SymbolB",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<byte>(
                name: "SymbolG",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<int>(
                name: "SymbolPosY",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte>(
                name: "SymbolR",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<float>(
                name: "SymbolScale",
                table: "Guilds",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BackgroundB",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "BackgroundG",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "BackgroundR",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "LogoBackground",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "LogoSymbol",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "SymbolB",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "SymbolG",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "SymbolPosY",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "SymbolR",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "SymbolScale",
                table: "Guilds");
        }
    }
}
