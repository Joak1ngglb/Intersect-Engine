using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.Sqlite.Game
{
    /// <inheritdoc />
    public partial class AddPetsgame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PetBaseId",
                table: "Items",
                type: "TEXT",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Pets",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Species = table.Column<string>(type: "TEXT", nullable: true),
                    RequiredMaturity = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiredEnergy = table.Column<int>(type: "INTEGER", nullable: false),
                    RequiredMood = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxBreedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    DefaultPersonality = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    Immunities = table.Column<string>(type: "TEXT", nullable: true),
                    Damage = table.Column<int>(type: "INTEGER", nullable: false),
                    DamageType = table.Column<int>(type: "INTEGER", nullable: false),
                    CritChance = table.Column<int>(type: "INTEGER", nullable: false),
                    CritMultiplier = table.Column<double>(type: "REAL", nullable: false),
                    AttackSpeedModifier = table.Column<int>(type: "INTEGER", nullable: false),
                    AttackSpeedValue = table.Column<int>(type: "INTEGER", nullable: false),
                    AttackAnimation = table.Column<Guid>(type: "TEXT", nullable: false),
                    DeathAnimation = table.Column<Guid>(type: "TEXT", nullable: false),
                    Sprite = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    AggroList = table.Column<string>(type: "TEXT", nullable: true),
                    AttackAllies = table.Column<bool>(type: "INTEGER", nullable: false),
                    Folder = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Scaling = table.Column<int>(type: "INTEGER", nullable: false),
                    ScalingStat = table.Column<int>(type: "INTEGER", nullable: false),
                    SightRange = table.Column<int>(type: "INTEGER", nullable: false),
                    Spells = table.Column<string>(type: "TEXT", nullable: true),
                    DefaultBehavior = table.Column<int>(type: "INTEGER", nullable: false),
                    TimeCreated = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pets", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pets");

            migrationBuilder.DropColumn(
                name: "PetBaseId",
                table: "Items");
        }
    }
}
