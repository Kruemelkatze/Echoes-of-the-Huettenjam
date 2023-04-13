using UnityEngine;

namespace Extensions
{
    public static class MathfExt
    {
        public static float FromMaxAbs(float a, float b)
        {
            var absA = Mathf.Abs(a);
            var absB = Mathf.Abs(b);

            return absA >= absB ? a : b;
        }

        public static float FromMinAbs(float a, float b)
        {
            var absA = Mathf.Abs(a);
            var absB = Mathf.Abs(b);

            return absA >= absB ? b : a;
        }

        public static float MaxAbs(float a, float b)
        {
            var absA = Mathf.Abs(a);
            var absB = Mathf.Abs(b);

            return absA > absB ? absA : absB;
        }

        public static float MinAbs(float a, float b)
        {
            var absA = Mathf.Abs(a);
            var absB = Mathf.Abs(b);

            return absA < absB ? absA : absB;
        }

        public static float MaxWithSign(float a, float b, float sign, bool maxAbsWhenSignZero = false)
        {
            if (sign == 0)
                return maxAbsWhenSignZero ? FromMaxAbs(a, b) : 0;

            return sign < 0 ? Mathf.Min(a, b) : Mathf.Max(a, b);
        }

        public static float Remap(this float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}