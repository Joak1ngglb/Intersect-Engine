using System;
using System.Collections.Generic;
using Intersect.Server.Networking;
using Intersect.Enums;
using Intersect.Server.Localization;
using Intersect.Config;
using Intersect.Framework.Core.Config;

namespace Intersect.Server.Entities
{
    public partial class Player : Entity
    {
        public void AddJobPoints(JobType jobType, int points)
        {
            if (Jobs.TryGetValue(jobType, out var job))
            {
                job.AddJobPoints(points);
                PacketSender.SendJobSync(this); // Sincroniza los datos con el cliente
            }
            else
            {
                PacketSender.SendChatMsg(this, $"Error: El trabajo '{jobType}' no está inicializado.", ChatMessageType.Error);
            }
        }

        public bool SpendJobPoints(JobType jobType, int points)
        {
            if (Jobs.TryGetValue(jobType, out var job))
            {
                if (job.SpendJobPoints(points))
                {
                    PacketSender.SendJobSync(this); // Sincroniza los datos con el cliente
                    return true;
                }
                else
                {
                    PacketSender.SendChatMsg(this, "No tienes suficientes puntos de trabajo.", ChatMessageType.Error);
                }
            }
            else
            {
                PacketSender.SendChatMsg(this, $"Error: El trabajo '{jobType}' no está inicializado.", ChatMessageType.Error);
            }
            return false;
        }

        public long GetJobPoints(JobType jobType)
        {
            return Jobs.TryGetValue(jobType, out var job) ? job.JobPoints : 0;
        }
        public void RecalculateJobPoints(JobType jobType)
        {
            if (Jobs.TryGetValue(jobType, out var job))
            {
                // Calcula los puntos esperados basados en el nivel del trabajo
                var expectedPoints = job.JobLevel * Options.JobPointGainPerLevel;

                // Si faltan puntos, ajusta
                if (job.JobPoints < expectedPoints)
                {
                    job.JobPoints += expectedPoints - job.JobPoints;
                }

                // Si sobran puntos (por errores o ajustes), corrige
                else if (job.JobPoints > expectedPoints)
                {
                    job.JobPoints = expectedPoints;
                }

                // Sincroniza los cambios con el cliente
                PacketSender.SendJobSync(this);
            }
        }

        public bool UnlockJobSkill(JobType jobType, Guid skillId)
        {
            if (Jobs.TryGetValue(jobType, out var job))
            {
                // Encuentra la habilidad asociada
                var skill = job.Skills.FirstOrDefault(s => s.SkillId == skillId);

                if (skill == null)
                {
                    PacketSender.SendChatMsg(this, $"Error: La habilidad con ID {skillId} no existe en el trabajo {jobType}.", ChatMessageType.Error);
                    return false;
                }

                if (skill.Unlocked)
                {
                    PacketSender.SendChatMsg(this, "La habilidad ya está desbloqueada.", ChatMessageType.Error);
                    return false;
                }

                if (job.JobPoints < skill.Cost)
                {
                    PacketSender.SendChatMsg(this, "No tienes suficientes puntos de trabajo para desbloquear esta habilidad.", ChatMessageType.Error);
                    return false;
                }

                if (job.JobLevel < skill.RequiredLevel)
                {
                    PacketSender.SendChatMsg(this, $"Debes ser nivel {skill.RequiredLevel} en {jobType} para desbloquear esta habilidad.", ChatMessageType.Error);
                    return false;
                }

                // Desbloquea la habilidad y gasta puntos
                skill.Unlocked = true;
                job.JobPoints -= skill.Cost;

                // Sincroniza cambios con el cliente
                PacketSender.SendJobSync(this);
                PacketSender.SendChatMsg(this, $"¡Habilidad '{skill.Name}' desbloqueada con éxito!", ChatMessageType.Notice);

                return true;
            }

            PacketSender.SendChatMsg(this, $"Error: El trabajo '{jobType}' no está inicializado.", ChatMessageType.Error);
            return false;
        }


    }

}