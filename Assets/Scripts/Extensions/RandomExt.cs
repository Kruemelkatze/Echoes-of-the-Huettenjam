using UnityEngine;

namespace Extensions
{
    public static class RandomExt
    {
        public static float Range(Vector2 range) => Random.Range(range.x, range.y);

        public static Vector3 PointInBounds(Bounds bounds)
        {
            var x = Random.Range(bounds.min.x, bounds.max.x);
            var y = Random.Range(bounds.min.y, bounds.max.y);
            var z = Random.Range(bounds.min.z, bounds.max.z);

            return new Vector3(x, y, z);
        }

        public static int Sign()
        {
            return Random.value <= 0.5f ? -1 : 1;
        }

        public static void Shuffle<T>(T[] array)
        {
            var n = array.Length;
            while (n > 1)
            {
                var k = Random.Range(0, n--);
                var temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }
}