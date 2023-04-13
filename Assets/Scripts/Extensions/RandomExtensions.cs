using UnityEngine;

namespace Extensions
{
    public static class RandomExtensions
    {
        public static float Range(Vector2 range) => Random.Range(range.x, range.y);

        public static Vector3 PointInBounds(Bounds bounds)
        {
            var x = Random.Range(bounds.min.x, bounds.max.x);
            var y = Random.Range(bounds.min.y, bounds.max.y);
            var z = Random.Range(bounds.min.z, bounds.max.z);

            return new Vector3(x, y, z);
        }
    }
}