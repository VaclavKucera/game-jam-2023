using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Library
{
    public static class RandomGeneration
    {
        private static readonly Vector3 DefaultCenter = Vector3.zero;
        private const float DefaultRadius = 3.9f;
        private const float MinRadius = 2.2f;

        public static Vector3 RandomPosition(Vector3? center = null, float? radius = null)
        {
            var r = Math.Max(radius ?? DefaultRadius, MinRadius);
            var c = center ?? DefaultCenter;
            return new Vector3(
                Random.Range(c.x - r, c.x + r),
                Random.Range(c.y - r, c.y + r),
                Random.Range(c.z - r, c.z + r)
            );
        }
    }
}
