using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.Sqlite.Player
{
    /// <inheritdoc />
    public partial class RemoveEnchantlevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Market_Listings_Players_PlayerId",
                table: "Market_Listings");

            migrationBuilder.DropIndex(
                name: "IX_Market_Listings_PlayerId",
                table: "Market_Listings");

            migrationBuilder.DropColumn(
                name: "EnchantmentLevel",
                table: "Player_Items");

            migrationBuilder.DropColumn(
                name: "EnchantmentLevel",
                table: "Player_Bank");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Market_Listings");

            migrationBuilder.DropColumn(
                name: "SymbolPosY",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "SymbolScale",
                table: "Guilds");

            migrationBuilder.DropColumn(
                name: "EnchantmentLevel",
                table: "Guild_Bank");

            migrationBuilder.DropColumn(
                name: "EnchantmentLevel",
                table: "Bag_Items");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<Guid>(
                name: "PlayerId",
                table: "Market_Listings",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SymbolPosY",
                table: "Guilds",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "SymbolScale",
                table: "Guilds",
                type: "REAL",
                nullable: false,
                defaultValue: 0f);

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

            migrationBuilder.CreateIndex(
                name: "IX_Market_Listings_PlayerId",
                table: "Market_Listings",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Market_Listings_Players_PlayerId",
                table: "Market_Listings",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
