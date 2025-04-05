using Intersect.Client.Framework.Graphics;
using System;

namespace Intersect.Utilities
{
    public static class GraphicsHelper
    {
        public static GameTexture Compose(GameTexture symbol, GameTexture background)
        {
            if (symbol == null || background == null)
                return null; // O maneja este caso según tus necesidades

            int width = background.Width;
            int height = background.Height;

            // Crea una textura compuesta a partir del fondo.
            GameTexture composed = new ConcreteGameTexture("ComposedTexture", width, height);

            // Copia el fondo en la textura compuesta.
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    composed.SetColor(x, y, background.GetPixel(x, y));
                }
            }

            // Calcula la posición para centrar el símbolo.
            int symbolWidth = symbol.Width;
            int symbolHeight = symbol.Height;
            int offsetX = (width - symbolWidth) / 2;
            int offsetY = (height - symbolHeight) / 2;

            // Superpone el símbolo con blending alfa.
            for (int y = 0; y < symbolHeight; y++)
            {
                for (int x = 0; x < symbolWidth; x++)
                {
                    Color symbolColor = symbol.GetPixel(x, y);
                    Color bgColor = composed.GetPixel(offsetX + x, offsetY + y);
                    Color blended = AlphaBlend(bgColor, symbolColor);
                    composed.SetColor(offsetX + x, offsetY + y, blended);
                }
            }

            return composed;
        }
        public static GameTexture Recolor(GameTexture texture, Color newColor)
        {
            if (texture == null)
            {
                Console.WriteLine("Recolor: La textura proporcionada es null.");
                return null;
            }

            // Verifica que las dimensiones sean válidas
            if (texture.Width <= 0 || texture.Height <= 0)
            {
                Console.WriteLine($"Recolor: Dimensiones inválidas ({texture.Width}x{texture.Height}) para la textura '{texture.Name}'.");
                return texture;
            }

            var recolored = new ConcreteGameTexture(texture.Name, texture.Width, texture.Height);
            for (int y = 0; y < texture.Height; y++)
            {
                for (int x = 0; x < texture.Width; x++)
                {
                    Color original = texture.GetPixel(x, y);
                    // Si el píxel es visible, se recolorea; de lo contrario, se copia
                    if (original.A > 0)
                    {
                        recolored.SetColor(x, y, new Color(newColor.R, newColor.G, newColor.B, original.A));
                    }
                    else
                    {
                        recolored.SetColor(x, y, original);
                    }
                }
            }
            return recolored;
        }


        // Función de blending alfa simple: mezcla el color del símbolo (overlay) sobre el fondo.
        private static Color AlphaBlend(Color background, Color overlay)
        {
            // Normaliza la opacidad del color del símbolo.
            float alpha = overlay.A / 255f;

            // Calcula cada componente del color mezclado.
            int r = (int)(overlay.R * alpha + background.R * (1 - alpha));
            int g = (int)(overlay.G * alpha + background.G * (1 - alpha));
            int b = (int)(overlay.B * alpha + background.B * (1 - alpha));
            // Puedes decidir cómo manejar la componente alfa; aquí se usa el valor máximo.
            int a = Math.Max(background.A, overlay.A);

            return new Color(r, g, b, a);
        }
    }

    public class ConcreteGameTexture : GameTexture
    {
        private Color[,] pixels;

        public ConcreteGameTexture(string name, int width, int height) : base(name)
        {
            pixels = new Color[width, height];
            Width = width;
            Height = height;
        }

        public override int Width { get; }
        public override int Height { get; }

        public override Color GetPixel(int x, int y)
        {
            return pixels[x, y];
        }

        public override void SetColor(int x, int y, Color color)
        {
            pixels[x, y] = color;
        }

        public override object? GetTexture()
        {
            // Implementación específica según tu plataforma
            return null;
        }

        public override GameTexturePackFrame? GetTexturePackFrame()
        {
            // Implementación específica según tu plataforma
            return null;
        }
    }
}
