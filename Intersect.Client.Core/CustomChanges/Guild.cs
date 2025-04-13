

using Intersect.Client.Networking;

namespace Intersect.Client.Entities;

public partial class Player
{
    public string GuildBackgroundFile { get; set; }
    public byte GuildBackgroundR { get; set; }
    public byte GuildBackgroundG { get; set; }
    public byte GuildBackgroundB { get; set; } = 255;

    public string GuildSymbolFile { get; set; }
    public byte GuildSymbolR { get; set; }
    public byte GuildSymbolG { get; set; }
    public byte GuildSymbolB { get; set; }

    public float GuildXpContribution { get; set; }

    public void GetLogo(
     string backgroundFile,
     byte backgroundR,
     byte backgroundG,
     byte backgroundB,
     string symbolFile,
     byte symbolR,
     byte symbolG,
     byte symbolB
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
