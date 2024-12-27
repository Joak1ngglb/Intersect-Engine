using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Intersect.Server.Entities
{
    public partial class Player : Entity
    {
        /// <summary>
        /// Calcula los puntos de experiencia modificados en función de la diferencia de niveles entre el jugador y el enemigo.
        /// </summary>
        /// <param name="enemyLevel">El nivel del enemigo.</param>
        /// <param name="baseExp">Los puntos de experiencia base ganados.</param>
        /// <param name="playerLevel">El nivel del jugador (opcional, con un valor predeterminado de 0).</param>
        /// <returns>Los puntos de experiencia modificados.</returns>
        public long ExpModifiedByLevel(int enemyLevel, long baseExp, int playerLevel = 0)
        {
            // Calcula la diferencia de niveles entre el jugador y el enemigo.
            int levelDiff = playerLevel == 0 ? Level - enemyLevel : playerLevel - enemyLevel;
            // Inicializa el multiplicador de experiencia como 1.0 (sin modificación).
            float expMultiplier = 1.0f;
            int reductionPercentage = 0;
            // Aplica modificadores basados en la diferencia de niveles.
            if (levelDiff >= 4 && levelDiff < 6)
            {
                expMultiplier = 0.8f; // Reducción del 20%
                reductionPercentage = 20;
            }
            else if (levelDiff >= 6 && levelDiff < 10)
            {
                expMultiplier = 0.6f; // Reducción del 40%
                reductionPercentage = 40;
            }
            else if (levelDiff >= 10)
            {
                expMultiplier = 0.2f; // Reducción del 80%
                reductionPercentage = 80;
            }
            // Calcula la experiencia modificada.
            long modifiedExp = (long)(baseExp * expMultiplier);

            // Mostrar un mensaje al jugador sobre la experiencia obtenida y la reducción aplicada.
            ShowExpGainMessage(baseExp, modifiedExp, reductionPercentage);

            // Mostrar un mensaje si el jugador está matando enemigos de nivel mucho menor.
            if (levelDiff >= 4)
            {
                ShowLowLevelKillMessage();
            }

            return modifiedExp;
        }
        /// <summary>
        /// Muestra un mensaje al jugador sobre la experiencia ganada y el porcentaje reducido.
        /// </summary>
        private void ShowExpGainMessage(long baseExp, long modifiedExp, int reductionPercentage)
        {
            string message = reductionPercentage > 0
                ? $"Has ganado {modifiedExp} puntos de experiencia (reducción del {reductionPercentage}% por diferencia de nivel)."
                : $"Has ganado {modifiedExp} puntos de experiencia.";
            SendMessageToPlayer(message, MessageType.Info);
        }

        /// <summary>
        /// Muestra un mensaje al jugador indicando que está matando enemigos de menor nivel.
        /// </summary>
        private void ShowLowLevelKillMessage()
        {
            string message = "¡Estás matando enemigos de menor nivel que el tuyo! Considera enfrentarte a desafíos más adecuados.";
            SendMessageToPlayer(message, MessageType.Warning);
        }

        /// <summary>
        /// Envía un mensaje al jugador.
        /// </summary>
        /// <param name="message">El mensaje a enviar.</param>
        /// <param name="type">El tipo de mensaje (por ejemplo, advertencia, información, etc.).</param>
        private void SendMessageToPlayer(string message, MessageType type)
        {
            // Implementa aquí la lógica para enviar el mensaje al jugador.
            // Esto puede depender del sistema de mensajería de tu juego.
            Console.WriteLine($"[{type}] {message}");
        }
    }

    /// <summary>
    /// Enumeración para los tipos de mensajes.
    /// </summary>
    public enum MessageType
    {
        Info,
        Warning,
        Error
    }
}