using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.Sqlite.Player
{
    /// <inheritdoc />
    public partial class AddItemsEnchantment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EnchantmentLevel",
                table: "Player_Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnchantmentLevel",
                table: "Player_Bank",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnchantmentLevel",
                table: "Guild_Bank",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EnchantmentLevel",
                table: "Bag_Items",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EnchantmentLevel",
                table: "Player_Items");

            migrationBuilder.DropColumn(
                name: "EnchantmentLevel",
                table: "Player_Bank");

            migrationBuilder.DropColumn(
                name: "EnchantmentLevel",
                table: "Guild_Bank");

            migrationBuilder.DropColumn(
                name: "EnchantmentLevel",
                table: "Bag_Items");
        }
    }
}
