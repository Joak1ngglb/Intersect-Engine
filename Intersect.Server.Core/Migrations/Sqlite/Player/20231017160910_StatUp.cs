using System;
using Microsoft.EntityFrameworkCore.Migrations;
#nullable disable
namespace Intersect.Server.Migrations.Sqlite.Player
{
    public partial class StatUp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Dodaj nowe elementy do tablic JSON, jeśli długość tablicy wynosi 5
            migrationBuilder.Sql(@"
                UPDATE Players
                SET BaseStats = json_insert(BaseStats, '$[5]', 0, '$[6]', 0, '$[7]', 0)
                WHERE json_array_length(BaseStats) = 5;
            ");
            migrationBuilder.Sql(@"
                UPDATE Players
                SET StatPointAllocations = json_insert(StatPointAllocations, '$[5]', 0, '$[6]', 0, '$[7]', 0)
                WHERE json_array_length(StatPointAllocations) = 5;
            ");
            migrationBuilder.Sql(@"
                UPDATE Bag_Items
                SET ItemProperties = json_insert(ItemProperties, '$.StatModifiers[5]', 0, '$.StatModifiers[6]', 0, '$.StatModifiers[7]', 0)
                WHERE json_array_length(json_extract(ItemProperties, '$.StatModifiers')) = 5;
            ");
            migrationBuilder.Sql(@"
                UPDATE Player_Bank
                SET ItemProperties = json_insert(ItemProperties, '$.StatModifiers[5]', 0, '$.StatModifiers[6]', 0, '$.StatModifiers[7]', 0)
                WHERE json_array_length(json_extract(ItemProperties, '$.StatModifiers')) = 5;
            ");
            migrationBuilder.Sql(@"
                UPDATE Player_Items
                SET ItemProperties = json_insert(ItemProperties, '$.StatModifiers[5]', 0, '$.StatModifiers[6]', 0, '$.StatModifiers[7]', 0)
                WHERE json_array_length(json_extract(ItemProperties, '$.StatModifiers')) = 5;
            ");
            migrationBuilder.Sql(@"
            UPDATE Player_Hotbar
            SET PreferredStatBuffs = json_insert(PreferredStatBuffs, '$[5]', 0, '$[6]', 0, '$[7]', 0)
            WHERE json_array_length(PreferredStatBuffs) = 5
            ");
            migrationBuilder.Sql(@"
                UPDATE Guild_Bank
                SET ItemProperties = json_insert(ItemProperties, '$.StatModifiers[5]', 0, '$.StatModifiers[6]', 0, '$.StatModifiers[7]', 0)
                WHERE json_array_length(json_extract(ItemProperties, '$.StatModifiers')) = 5;
            ");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Usuń dodane elementy z tablic JSON, jeśli długość tablicy wynosi 8
            migrationBuilder.Sql(@"
                UPDATE Players
                SET BaseStats = json_remove(BaseStats, '$[5]', '$[6]', '$[7]')
                WHERE json_array_length(BaseStats) = 8;
            ");
            migrationBuilder.Sql(@"
                UPDATE Players
                SET StatPointAllocations = json_remove(StatPointAllocations, '$[5]', '$[6]', '$[7]')
                WHERE json_array_length(StatPointAllocations) = 8;
            ");
            migrationBuilder.Sql(@"
            UPDATE Player_Hotbar
            SET PreferredStatBuffs = json_remove(PreferredStatBuffs, '$[5]', '$[6]', '$[7]')
            WHERE json_array_length(PreferredStatBuffs) = 8
        ");
            migrationBuilder.Sql(@"
                UPDATE Bag_Items
                SET ItemProperties = json_remove(ItemProperties, '$.StatModifiers[7]', '$.StatModifiers[6]', '$.StatModifiers[5]')
                WHERE json_array_length(json_extract(ItemProperties, '$.StatModifiers')) = 8;
            ");
            migrationBuilder.Sql(@"
                UPDATE Player_Bank
                SET ItemProperties = json_remove(ItemProperties, '$.StatModifiers[7]', '$.StatModifiers[6]', '$.StatModifiers[5]')
                WHERE json_array_length(json_extract(ItemProperties, '$.StatModifiers')) = 8;
            ");
            migrationBuilder.Sql(@"
                UPDATE Player_Items
                SET ItemProperties = json_remove(ItemProperties, '$.StatModifiers[7]', '$.StatModifiers[6]', '$.StatModifiers[5]')
                WHERE json_array_length(json_extract(ItemProperties, '$.StatModifiers')) = 8;
            ");
            migrationBuilder.Sql(@"
                UPDATE Guild_Bank
                SET ItemProperties = json_remove(ItemProperties, '$.StatModifiers[7]', '$.StatModifiers[6]', '$.StatModifiers[5]')
                WHERE json_array_length(json_extract(ItemProperties, '$.StatModifiers')) = 8;
            ");
        }
    }
}