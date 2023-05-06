using UnityEngine;

namespace Library
{
    public static class RandomGeneration
    {
        private static readonly Vector3 DefaultCenter = Vector3.zero;
        private const float DefaultRadius = 5;

        public static Vector3 RandomPosition(Vector3? center = null, float? radius = null)
        {
            var r = radius ?? DefaultRadius;
            var c = center ?? DefaultCenter;
            return new Vector3(
                Random.Range(c.x, c.x + r),
                Random.Range(c.y, c.y + r),
                Random.Range(c.z, c.z + r)
            );
        }
    }
}
