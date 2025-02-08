using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Intersect.Server.Migrations.Sqlite.Player
{
    /// <inheritdoc />
    public partial class AddPlayerPets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Players",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PetId",
                table: "Player_Spells",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PetId",
                table: "Player_Items",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Maturity = table.Column<int>(type: "INTEGER", nullable: false),
                    Energy = table.Column<int>(type: "INTEGER", nullable: false),
                    Mood = table.Column<int>(type: "INTEGER", nullable: false),
                    BreedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSterile = table.Column<bool>(type: "INTEGER", nullable: false),
                    PetGender = table.Column<int>(type: "INTEGER", nullable: false),
                    Personality = table.Column<int>(type: "INTEGER", nullable: false),
                    Rarity = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<long>(type: "INTEGER", nullable: false),
                    Behavior = table.Column<int>(type: "INTEGER", nullable: false),
                    Vital = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSummoned = table.Column<bool>(type: "INTEGER", nullable: false),
                    OwnerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MapId = table.Column<Guid>(type: "TEXT", nullable: false),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    Z = table.Column<int>(type: "INTEGER", nullable: false),
                    Dir = table.Column<int>(type: "INTEGER", nullable: false),
                    Sprite = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    Face = table.Column<string>(type: "TEXT", nullable: true),
                    Vitals = table.Column<string>(type: "TEXT", nullable: true),
                    BaseStats = table.Column<string>(type: "TEXT", nullable: true),
                    StatPointAllocations = table.Column<string>(type: "TEXT", nullable: true),
                    NameColor = table.Column<string>(type: "TEXT", nullable: true),
                    HeaderLabel = table.Column<string>(type: "TEXT", nullable: true),
                    FooterLabel = table.Column<string>(type: "TEXT", nullable: true),
                    SpellCooldowns = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pet_Players_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PetBase",
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
                    table.PrimaryKey("PK_PetBase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Player_Pets",
                columns: table => new
                {
                    PetId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PlayerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PetBaseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Experience = table.Column<long>(type: "INTEGER", nullable: false),
                    Gender = table.Column<int>(type: "INTEGER", nullable: false),
                    Personality1 = table.Column<int>(type: "INTEGER", nullable: false),
                    Energy = table.Column<int>(type: "INTEGER", nullable: false),
                    Mood = table.Column<int>(type: "INTEGER", nullable: false),
                    Maturity = table.Column<int>(type: "INTEGER", nullable: false),
                    BreedCount = table.Column<int>(type: "INTEGER", nullable: false),
                    IsSterile = table.Column<bool>(type: "INTEGER", nullable: false),
                    Stats = table.Column<string>(type: "TEXT", nullable: true),
                    Vital = table.Column<string>(type: "TEXT", nullable: true),
                    Personality = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player_Pets", x => x.PetId);
                    table.ForeignKey(
                        name: "FK_Player_Pets_PetBase_PetBaseId",
                        column: x => x.PetBaseId,
                        principalTable: "PetBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Player_Pets_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Player_Spells_PetId",
                table: "Player_Spells",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Items_PetId",
                table: "Player_Items",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_Pet_OwnerId",
                table: "Pet",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Pets_PetBaseId",
                table: "Player_Pets",
                column: "PetBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_Pets_PlayerId",
                table: "Player_Pets",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Items_Pet_PetId",
                table: "Player_Items",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Spells_Pet_PetId",
                table: "Player_Spells",
                column: "PetId",
                principalTable: "Pet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Items_Pet_PetId",
                table: "Player_Items");

            migrationBuilder.DropForeignKey(
                name: "FK_Player_Spells_Pet_PetId",
                table: "Player_Spells");

            migrationBuilder.DropTable(
                name: "Pet");

            migrationBuilder.DropTable(
                name: "Player_Pets");

            migrationBuilder.DropTable(
                name: "PetBase");

            migrationBuilder.DropIndex(
                name: "IX_Player_Spells_PetId",
                table: "Player_Spells");

            migrationBuilder.DropIndex(
                name: "IX_Player_Items_PetId",
                table: "Player_Items");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Player_Spells");

            migrationBuilder.DropColumn(
                name: "PetId",
                table: "Player_Items");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Players",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "TEXT")
                .Annotation("Relational:ColumnOrder", 0);
        }
    }
}
