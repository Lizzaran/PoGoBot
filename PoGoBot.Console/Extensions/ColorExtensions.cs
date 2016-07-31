using System.Drawing;

namespace PoGoBot.Console.Extensions
{
    internal static class ColorExtensions
    {
        private static float Lerp(float start, float end, float amount)
        {
            var difference = end - start;
            var adjusted = difference*amount;
            return start + adjusted;
        }

        public static Color Lerp(this Color colour, Color to, float amount)
        {
            float sr = colour.R, sg = colour.G, sb = colour.B;
            float er = to.R, eg = to.G, eb = to.B;
            byte r = (byte) Lerp(sr, er, amount),
                g = (byte) Lerp(sg, eg, amount),
                b = (byte) Lerp(sb, eb, amount);
            return Color.FromArgb(r, g, b);
        }
    }
}