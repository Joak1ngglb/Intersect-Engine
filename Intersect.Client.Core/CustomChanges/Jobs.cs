using Intersect.Client.Core;
using Intersect.Client.Core.Controls;
using Intersect.Client.Entities.Events;
using Intersect.Client.Entities.Projectiles;
using Intersect.Client.Framework.Entities;
using Intersect.Client.Framework.GenericClasses;
using Intersect.Client.Framework.Items;
using Intersect.Client.General;
using Intersect.Client.Interface.Game.Chat;
using Intersect.Client.Interface.Game.EntityPanel;
using Intersect.Client.Interface.Shared;
using Intersect.Client.Items;
using Intersect.Client.Localization;
using Intersect.Client.Maps;
using Intersect.Client.Networking;
using Intersect.Config;
using Intersect.Config.Guilds;
using Intersect.Configuration;
using Intersect.Enums;
using Intersect.Extensions;
using Intersect.Framework.Core.Config;
using Intersect.GameObjects;
using Intersect.GameObjects.Maps;
using Intersect.Logging;
using Intersect.Network.Packets.Server;
using Intersect.Utilities;


namespace Intersect.Client.Entities;

public partial class Player
{
    // Inicializa los diccionarios al crear el jugador
    public Dictionary<JobType, int> JobLevel { get; set; } = [];
    public Dictionary<JobType, long> JobExp { get; set; } = [];
    public Dictionary<JobType, long> JobExpToNextLevel { get; set; } = [];
 

    public void UpdateJobsFromPacket(Dictionary<JobType, JobData> jobData)
    {
        if (jobData == null)
        {
            PacketSender.SendChatMsg("Error: El paquete de datos de trabajos está vacío.", 5);
            return;
        }

        foreach (var job in jobData)
        {
            var jobType = job.Key;
            var jobDetails = job.Value;

            // Asegurar inicialización
            if (!JobLevel.ContainsKey(jobType))
            {
                JobLevel[jobType] = 1;
            }

            if (!JobExp.ContainsKey(jobType))
            {
                JobExp[jobType] = 0;
            }

            if (!JobExpToNextLevel.ContainsKey(jobType))
            {
                JobExpToNextLevel[jobType] = 100;
            }

            // Actualizar valores
            JobLevel[jobType] = jobDetails.Level;
            JobExp[jobType] = jobDetails.Experience;
            JobExpToNextLevel[jobType] = jobDetails.ExperienceToNextLevel;

            // Depuración en el cliente
          //  PacketSender.SendChatMsg($"Trabajo {jobType} actualizado: Nivel {jobDetails.Level}, Exp {jobDetails.Experience}/{jobDetails.ExperienceToNextLevel}", 5);
        }
    }

    public string GuildBackgroundFile { get; set; } 
    public byte GuildBackgroundR { get; set; } 
    public byte GuildBackgroundG { get; set; } 
    public byte GuildBackgroundB { get; set; } = 255;

    public string GuildSymbolFile { get; set; }
    public byte GuildSymbolR { get; set; } 
    public byte GuildSymbolG { get; set; }
    public byte GuildSymbolB { get; set; } 

    public int GuildSymbolPosY { get; set; } = 0;
    public float GuildSymbolScale { get; set; } = 1.0f;
    public float GuildXpContribution { get; set; }

    public void GetLogo(
     string backgroundFile,
     byte backgroundR,
     byte backgroundG,
     byte backgroundB,
     string symbolFile,
     byte symbolR,
     byte symbolG,
     byte symbolB,
     int symbolPosY,
     float symbolScale
 )
    {
        // Validaciones básicas
        if (string.IsNullOrEmpty(backgroundFile))
        {
            PacketSender.SendChatMsg("Error: El fondo del logo está vacío.", 5);
            return;
        }
        if (string.IsNullOrEmpty(symbolFile))
        {
            PacketSender.SendChatMsg("Error: El símbolo del logo está vacío.", 5);
            return;
        }

        // Guardar los valores en propiedades
        GuildBackgroundFile = backgroundFile;
        GuildBackgroundR = backgroundR;
        GuildBackgroundG = backgroundG;
        GuildBackgroundB = backgroundB;

        GuildSymbolFile = symbolFile;
        GuildSymbolR = symbolR;
        GuildSymbolG = symbolG;
        GuildSymbolB = symbolB;

        GuildSymbolPosY = symbolPosY;
        GuildSymbolScale = symbolScale;

       /* // Mensaje de depuración (opcional)
        PacketSender.SendChatMsg(
            $"Logo procesado:\n" +
            $"Fondo={backgroundFile} (R={backgroundR},G={backgroundG},B={backgroundB}),\n" +
            $"Símbolo={symbolFile} (R={symbolR},G={symbolG},B={symbolB}),\n" +
            $"PosY={symbolPosY}, Escala={symbolScale}.",
            5
        );*/
    }
}
