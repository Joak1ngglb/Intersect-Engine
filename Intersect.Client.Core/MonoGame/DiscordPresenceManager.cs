using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using Intersect.Client.Framework.Content;
using Intersect.Client.General;
using Intersect.GameObjects;
using Intersect.Client.Maps;

namespace Intersect.Client.MonoGame;
public static class DiscordPresenceManager
{
    private static DiscordRpcClient client;
    private static bool isUpdatingPresence = false;
    private static string currentState = "In the game";
    private static DateTime startTime;
    private static bool isInitialized = false;

    public static void Initialize(string applicationId)
    {
        if (!isInitialized)
        {
            client = new DiscordRpcClient(applicationId);
            client.Initialize();
            startTime = DateTime.UtcNow;
            isInitialized = true;
        }
    }

    private static async void UpdatePresenceInternal()
    {
        if (!isUpdatingPresence)
        {
            isUpdatingPresence = true;

            try
            {
                // Obtener el nombre del mapa actual
                string mapName = "Unknown";
                var currentMap = MapInstance.Get(Globals.Me.MapId);
                if (currentMap != null)
                {
                    mapName = currentMap.Name;
                }

                // Formatear la información del personaje con comas como separadores
                var details = $"Name: {Globals.Me.Name}, " +
                             $"Class: {ClassBase.GetName(Globals.Me.Class)}, " +
                             $"Lvl: {Globals.Me.Level}";

                var state = $"Map: {mapName}";

                var presence = new RichPresence
                {
                    Details = details,
                    State = state,
                    Assets = new Assets
                    {
                        LargeImageKey = "bbod",
                        LargeImageText = $"Location: {mapName}",
                        SmallImageKey = "gif",
                        SmallImageText = "Verified"
                    },
                    Timestamps = new Timestamps(startTime),
                };

                // Configurar los botones
                presence.Buttons = new Button[]
                {
                    new Button
                    {
                        Label = "Join our Discord!",
                        Url = "https://discord.gg/BdJwBKe3Fu"
                    },
                    new Button
                    {
                        Label = "Visit our Website!",
                        Url = "https://brokenbridge.online"
                    }
                };

                client.SetPresence(presence);

                await Task.Delay(5000);
                isUpdatingPresence = false;

                if (currentState == presence.State)
                {
                    UpdatePresenceInternal();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating Discord presence: {ex.Message}");
                isUpdatingPresence = false;
            }
        }
    }

    public static void UpdatePresence(string state)
    {
        currentState = state;
        UpdatePresenceInternal();
    }

    public static void Dispose()
    {
        if (isInitialized)
        {
            client?.Dispose();
            isInitialized = false;
        }
    }
}