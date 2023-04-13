using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class VectorExt
    {
        public static Quaternion Get2DRotation(this Vector2 direction)
        {
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
            return Quaternion.AngleAxis(angle, Vector3.forward);
        }

        public static Vector2 Get2DDirection(this Quaternion rotation, float? snapTo = null)
        {
            var angle = rotation.eulerAngles.z;

            if (snapTo.HasValue && snapTo.Value > 0)
            {
                angle = snapTo.Value * Mathf.Round(angle / snapTo.Value);
            }

            return Vector2.up.Rotate(angle);
        }

        public static float Get2DRotationAngle(this Vector2 direction, bool angleOffset = true)
        {
            var a = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            return angleOffset ? a - 90 : a;
        }

        public static Vector2 Rotate(this Vector2 point, float degrees)
        {
            var rad = degrees * Mathf.Deg2Rad;
            var s = Mathf.Sin(rad);
            var c = Mathf.Cos(rad);
            return new Vector2(
                point.x * c - point.y * s,
                point.x * s + point.y * c
            );
        }

        /// <summary>
        ///   <para>Returns the squared distance between a and b.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static float DistanceSquared(Vector2 a, Vector2 b)
        {
            var num1 = a.x - b.x;
            var num2 = a.y - b.y;
            return num1 * num1 + num2 * num2;
        }

        public static Vector2 GetWithMaxMagnitude(params Vector2[] vectors)
        {
            var max = Vector2.zero;
            var maxMagnSqr = 0f;
            foreach (var v in vectors)
            {
                max = v.sqrMagnitude > maxMagnSqr ? v : max;
            }

            return max;
        }

        public static bool Approximately(this Vector2 v1, Vector2 v2)
        {
            return Mathf.Approximately((v1 - v2).SqrMagnitude(), 0);
        }

        public static Vector2 Average(params Vector2[] vectors)
        {
            return Average((IEnumerable<Vector2>) vectors);
        }

        public static Vector2 Average(IEnumerable<Vector2> vectors)
        {
            var sumX = 0f;
            var sumY = 0f;
            var count = 0;

            foreach (var vector2 in vectors)
            {
                sumX += vector2.x;
                sumY += vector2.y;
                count++;
            }

            if (count == 0)
            {
                return Vector2.zero;
            }

            return new Vector2(sumX / count, sumY / count);
        }

        public static Vector2 ComponentWiseProduct(this Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x * v2.x, v1.y * v2.y);
        }

        public static Vector2 ComponentWiseProduct(this Vector2 v1, float x, float y)
        {
            return new Vector2(v1.x * x, v1.y * y);
        }

        public static Vector3 ComponentWiseProduct(this Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x * v2.x, v1.y * v2.y, v1.z * v2.z);
        }

        public static Vector3 ComponentWiseProduct(this Vector3 v1, float x, float y, float z)
        {
            return new Vector3(v1.x * x, v1.y * y, v1.z * z);
        }
    }
}