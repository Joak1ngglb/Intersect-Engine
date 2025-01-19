using MessagePack;
using Intersect.Enums;
using System.Collections.Generic;
using Intersect.Config;
using Intersect.Framework.Core.Config;

namespace Intersect.Network.Packets.Server;

[MessagePackObject]
public partial class JobSyncPacket : IntersectPacket
{
    // Constructor sin parámetros para MessagePack
    public JobSyncPacket()
    {
    }

    // Constructor para inicializar los datos
    public JobSyncPacket(Dictionary<JobType, JobData> jobs)
    {
        Jobs = jobs;
    }

    [Key(0)]
    public Dictionary<JobType, JobData> Jobs { get; set; }
}

[MessagePackObject]
public class JobData
{
    [Key(0)]
    public int Level { get; set; }

    [Key(1)]
    public long Experience { get; set; }

    [Key(2)]
    public long ExperienceToNextLevel { get; set; }
    [Key(3)]
    public int JobPoints { get; set; }
    [Key(4)]
    public List<SkillData> Skills { get; set; } // Cambiado de object a List<SkillData>
}
[MessagePackObject]
public class SkillData
{
    [Key(0)]
    public Guid SkillId { get; set; } // Identificador único

    [Key(1)]
    public string Name { get; set; } // Nombre de la habilidad

    [Key(2)]
    public int RequiredLevel { get; set; } // Nivel necesario para desbloquear

    [Key(3)]
    public int Cost { get; set; } // Costo en puntos

    [Key(4)]
    public bool Unlocked { get; set; } // Estado de desbloqueo

    [Key(5)]
    public JobSkillEffectType EffectType { get; set; } // Tipo de efecto

    [Key(6)]
    public float EffectValue { get; set; } // Valor del efecto (porcentaje o cantidad)
}