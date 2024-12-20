using Intersect.Server.Database;
using Intersect.Server.Migrations.SqlOnlyDataMigrations;
using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace Intersect.Server.Migrations.Sqlite.Game
{
    /// <inheritdoc />
    public partial class Statssup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Modify existing data in Items table
            migrationBuilder.Sql(@"
            UPDATE Items
            SET PercentageStatsGiven = json_insert(PercentageStatsGiven, '$[5]', 0, '$[6]', 0, '$[7]', 0)
            WHERE json_array_length(PercentageStatsGiven) = 5
        ");
            migrationBuilder.Sql(@"
            UPDATE Items
            SET StatsGiven = json_insert(StatsGiven, '$[5]', 0, '$[6]', 0, '$[7]', 0)
            WHERE json_array_length(StatsGiven) = 5
        ");
            // Modify existing data in Classes table
            migrationBuilder.Sql(@"
            UPDATE Classes
            SET BaseStats = json_insert(BaseStats, '$[5]', 0, '$[6]', 0, '$[7]', 0)
            WHERE json_array_length(BaseStats) = 8
        ");
            migrationBuilder.Sql(@"
            UPDATE Classes
            SET StatIncreases = json_insert(StatIncreases, '$[5]', 0, '$[6]', 0, '$[7]', 0)
            WHERE json_array_length(StatIncreases) = 5
        ");
            migrationBuilder.Sql(@"
            UPDATE Npcs
            SET Stats = json_insert(Stats, '$[5]', 0, '$[6]', 0, '$[7]', 0)
            WHERE json_array_length(Stats) = 5
        ");
            // Ensure StatDiff JSON array has 8 elements
            migrationBuilder.Sql(@"
            UPDATE Spells
            SET StatDiff = json_insert(StatDiff, '$[5]', 0, '$[6]', 0, '$[7]', 0)
            WHERE json_array_length(StatDiff) = 5
        ");
            // Ensure PercentageStatDiff JSON array has 8 elements
            migrationBuilder.Sql(@"
            UPDATE Spells
            SET PercentageStatDiff = json_insert(PercentageStatDiff, '$[5]', 0, '$[6]', 0, '$[7]', 0)
            WHERE json_array_length(PercentageStatDiff) = 5
        ");
            // Dodanie nowych kolumn do tabeli Items_EquipmentProperties z domyślną wartością 0
            migrationBuilder.AddColumn<int>(
                name: "StatRange_ArmorPenetration_LowRange",
                table: "Items_EquipmentProperties",
                type: "INTEGER",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "StatRange_ArmorPenetration_HighRange",
                table: "Items_EquipmentProperties",
                type: "INTEGER",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "StatRange_Vitality_LowRange",
                table: "Items_EquipmentProperties",
                type: "INTEGER",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "StatRange_Vitality_HighRange",
                table: "Items_EquipmentProperties",
                type: "INTEGER",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "StatRange_Wisdom_LowRange",
                table: "Items_EquipmentProperties",
                type: "INTEGER",
                nullable: true);
            migrationBuilder.AddColumn<int>(
                name: "StatRange_Wisdom_HighRange",
                table: "Items_EquipmentProperties",
                type: "INTEGER",
                nullable: true);
            migrationBuilder.Sql(@"
            UPDATE Items_EquipmentProperties
            SET StatRange_ArmorPenetration_LowRange = COALESCE(StatRange_ArmorPenetration_LowRange, 0),
                StatRange_ArmorPenetration_HighRange = COALESCE(StatRange_ArmorPenetration_HighRange, 0),
                StatRange_Vitality_LowRange = COALESCE(StatRange_Vitality_LowRange, 0),
                StatRange_Vitality_HighRange = COALESCE(StatRange_Vitality_HighRange, 0),
                StatRange_Wisdom_LowRange = COALESCE(StatRange_Wisdom_LowRange, 0),
                StatRange_Wisdom_HighRange = COALESCE(StatRange_Wisdom_HighRange, 0)
        ");
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert changes in Items_EquipmentProperties table
            migrationBuilder.DropColumn(
                name: "StatRange_ArmorPenetration_LowRange",
                table: "Items_EquipmentProperties");
            migrationBuilder.DropColumn(
                name: "StatRange_ArmorPenetration_HighRange",
                table: "Items_EquipmentProperties");
            migrationBuilder.DropColumn(
                name: "StatRange_Vitality_LowRange",
                table: "Items_EquipmentProperties");
            migrationBuilder.DropColumn(
                name: "StatRange_Vitality_HighRange",
                table: "Items_EquipmentProperties");
            migrationBuilder.DropColumn(
                name: "StatRange_Wisdom_LowRange",
                table: "Items_EquipmentProperties");
            migrationBuilder.DropColumn(
                name: "StatRange_Wisdom_HighRange",
                table: "Items_EquipmentProperties");
            // Revert data changes if necessary
            migrationBuilder.Sql(@"
            UPDATE Items
            SET PercentageStatsGiven = json_remove(PercentageStatsGiven, '$[5]', '$[6]', '$[7]')
            WHERE json_array_length(PercentageStatsGiven) = 8
        ");
            migrationBuilder.Sql(@"
            UPDATE Items
            SET StatsGiven = json_remove(StatsGiven, '$[5]', '$[6]', '$[7]')
            WHERE json_array_length(StatsGiven) = 8
        ");
            migrationBuilder.Sql(@"
            UPDATE Classes
            SET BaseStats = json_remove(BaseStats, '$[5]', '$[6]', '$[7]')
            WHERE json_array_length(BaseStats) = 8
        ");
            migrationBuilder.Sql(@"
            UPDATE Classes
            SET StatIncreases = json_remove(StatIncreases, '$[5]', '$[6]', '$[7]')
            WHERE json_array_length(StatIncreases) = 8
        ");
            migrationBuilder.Sql(@"
            UPDATE Npcs
            SET Stats = json_remove(Stats, '$[5]', '$[6]', '$[7]')
            WHERE json_array_length(Stats) = 8
        ");
            migrationBuilder.Sql(@"
            UPDATE Spells
            SET StatDiff = json_remove(StatDiff, '$[5]', '$[6]', '$[7]')
            WHERE json_array_length(StatDiff) = 8
        ");
            migrationBuilder.Sql(@"
            UPDATE Spells
            SET PercentageStatDiff = json_remove(PercentageStatDiff, '$[5]', '$[6]', '$[7]')
            WHERE json_array_length(PercentageStatDiff) = 8
        ");
        }
    }
}