using System.ComponentModel.DataAnnotations.Schema;
using Intersect.Enums;
using Intersect.Framework.Core.Config;
using Intersect.Models;
using Newtonsoft.Json;

namespace Intersect.GameObjects
{
    public partial class JobBase : DatabaseObject<JobBase>, IFolderable
    {
        // Constructor
        [JsonConstructor]
        public JobBase(Guid id) : base(id)
        {
            Name = "New Job";
        }

        public JobBase()
        {
            Name = "New Job";
        }

        // Nombre y tipo del trabajo (referencia)
        public string Name { get; set; }
        public JobType JobType { get; set; }

        // Descripción y representación gráfica
        public string Description { get; set; } = "";
        public string Icon { get; set; } = "";

        // Lista de habilidades desbloqueables
        [NotMapped]
        public List<JobSkill> Skills { get; set; } = new List<JobSkill>();

        [Column("Skills")]
        [JsonIgnore]
        public string JsonSkills
        {
            get => JsonConvert.SerializeObject(Skills);
            set => Skills = JsonConvert.DeserializeObject<List<JobSkill>>(value ?? "[]");
        }

        // Métodos para manipular las habilidades
        public void AddSkill(JobSkill skill)
        {
            if (Skills.Any(s => s.SkillId == skill.SkillId))
            {
                throw new InvalidOperationException("La habilidad ya existe en este trabajo.");
            }

            Skills.Add(skill);
        }

        public void RemoveSkill(Guid skillId)
        {
            var skill = Skills.FirstOrDefault(s => s.SkillId == skillId);
            if (skill == null)
            {
                throw new KeyNotFoundException("La habilidad no existe en este trabajo.");
            }

            Skills.Remove(skill);
        }

        public JobSkill GetSkill(Guid skillId)
        {
            return Skills.FirstOrDefault(s => s.SkillId == skillId);
        }

        // Categoría o carpeta para el editor
        public string Folder { get; set; } = "";

        public class JobSkill
        {
            public Guid SkillId { get; set; } = Guid.NewGuid(); // Identificador único de la habilidad
            public string Name { get; set; } = "New Skill";     // Nombre de la habilidad
            public int RequiredLevel { get; set; } = 1;         // Nivel requerido para desbloquear
            public int Cost { get; set; } = 1;                  // Costo en puntos para desbloquear
            public bool Unlocked { get; set; } = false;         // Estado de desbloqueo
            public JobSkillEffectType EffectType { get; set; }  // Tipo de efecto
            public float EffectValue { get; set; } = 0;         // Valor del efecto
            public string Description { get; set; }
        }
    }
}
