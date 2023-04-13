using UnityEngine;
using Random = System.Random;

namespace Extensions
{
    public static class ColorExt
    {
        private static Random _random = new Random();

        public static Color Random()
        {
            return new Color((float) _random.NextDouble(), (float) _random.NextDouble(), (float) _random.NextDouble(),
                1);
        }

        public static Color CloneAndSetAlpha(this Color color, float alpha) =>
            new Color(color.r, color.g, color.b, alpha);
    }
}