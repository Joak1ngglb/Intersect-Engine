using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

using Intersect.Enums;
using Intersect.Server.Entities;
using Intersect.Server.Networking;
using System.Collections.Concurrent;
using Intersect.Collections.Slotting;
using Microsoft.EntityFrameworkCore;
using Intersect.Network.Packets.Server;
using Intersect.GameObjects;
using Intersect.GameObjects.Maps;
using Intersect.Logging;
using Intersect.Utilities;
using Intersect.Server.Localization;
using Intersect.Server.Web.RestApi.Payloads;
using static Intersect.Server.Database.Logging.Entities.GuildHistory;
using Intersect.Config.Guilds;

namespace Intersect.Server.Database.PlayerData.Players;

/// <summary>
/// A class containing the definition of each guild, alongside the methods to use them.
/// </summary>
public partial class Guild
{
    // Nuevo sistema de niveles
    public int Level { get;  set; } = 1;
    public long Experience { get; set; } = 0;
    public long ExperienceToNextLevel => CalculateRequiredExperience(Level);
    public int MaxMembers => CalculateMaxMembers(Level);
    /// <summary>
    /// The name of the guild.
    /// </summary>
    public string Name { get; private set; }
    // Indica el archivo (o “ID”/“nombre”) para el fondo
    public string LogoBackground { get; set; } = string.Empty;

    // Colores RGB para el fondo
    public byte BackgroundR { get; set; } = 255;
    public byte BackgroundG { get; set; } = 255;
    public byte BackgroundB { get; set; } = 255;

    // Indica el archivo para el símbolo
    public string LogoSymbol { get; set; } = string.Empty;

    // Colores RGB para el símbolo
    public byte SymbolR { get; set; } = 255;
    public byte SymbolG { get; set; } = 255;
    public byte SymbolB { get; set; } = 255;

    // Parámetros para la posición/escala del símbolo, si deseas guardarlo
    public int SymbolPosY { get; set; } = 0;
    public float SymbolScale { get; set; } = 1.0f;
    /// <summary>
    /// Añade experiencia al gremio y verifica si puede subir de nivel.
    /// </summary>


    /// <summary>
    /// Calcula la XP necesaria para subir al siguiente nivel usando los valores configurables.
    /// </summary>
    private long CalculateRequiredExperience(int level)
    {
        return (long)(Options.Instance.Guild.BaseXP * Math.Pow(level, Options.Instance.Guild.GrowthFactor));
    }

    /// <summary>
    /// Calcula la cantidad máxima de miembros en un gremio según su nivel.
    /// </summary>
    private int CalculateMaxMembers(int level)
    {
        return Options.Instance.Guild.InitialMaxMembers + ((level - 1) * 5); // Aumenta 5 miembros por nivel
    }

    /// <summary>
    /// Añade experiencia al gremio y verifica si puede subir de nivel.
    /// </summary>
    public void AddExperience(long amount)
    {
        if (amount <= 0)
            return;

        Experience += amount;

        if (CheckLevelUp())
        {
            UpdateMemberList(); // Refrescar visualmente en los clientes
        }

        Save(); // Guardar en DB
    }
    private bool CheckLevelUp()
    {
        var levelUps = 0;

        while (Experience >= CalculateRequiredExperience(Level + levelUps) &&
                CalculateRequiredExperience(Level + levelUps) > 0)
        {
            Experience -= CalculateRequiredExperience(Level + levelUps);
            levelUps++;
        }

        if (levelUps <= 0)
            return false;

        LevelUp(false, levelUps);
        return true;
    }

    public void LevelUp(bool resetExperience = false, int levels = 1)
    {
        if (levels <= 0)
            return;

        for (int i = 0; i < levels; i++)
        {
            Level++;
            if (resetExperience)
            {
                Experience = 0;
            }

            // Mensaje a todos los miembros online
            foreach (var member in FindOnlineMembers())
            {
                PacketSender.SendChatMsg(member, $"¡El gremio ha subido al nivel {Level}!", ChatMessageType.Guild);
            }
        }

        UpdateMemberList();
        Save();
    }
    public void SetLevel(int level, bool resetExperience = false)
    {
        if (level < 1)
            return;

        Level = level;
        if (resetExperience)
            Experience = 0;

        UpdateMemberList();
        Save();
    }

}

