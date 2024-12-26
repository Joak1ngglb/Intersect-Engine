namespace Intersect.Client.Utilities;

public static partial class ColorHelper
{
    /// <summary>
    /// Interpolates between two colors by a given ratio.
    /// </summary>
    public static Color ColorInterpolate(Color color1, Color color2, float ratio)
    {
        byte a = (byte)((color2.A - color1.A) * ratio + color1.A);
        byte r = (byte)((color2.R - color1.R) * ratio + color1.R);
        byte g = (byte)((color2.G - color1.G) * ratio + color1.G);
        byte b = (byte)((color2.B - color1.B) * ratio + color1.B);
        return Color.FromArgb(a, r, g, b);
    }
}