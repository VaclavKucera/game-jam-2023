using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Library
{
    public static class RandomGeneration
    {
        private static readonly Vector2 DefaultCenter = Vector2.zero;
        private const float DefaultRadius = 3.9f;
        private const float MinRadius = 2.2f;

        public static Vector2 RandomPosition(Vector2? center = null, float? radius = null)
        {
            var r = radius ?? DefaultRadius;
            var c = center ?? DefaultCenter;
            var x = Random.Range(c.x + MinRadius, c.x + r);
            var y = Random.Range(c.y + MinRadius, c.y + r);
            var signX = Random.Range(0, 2) * 2 - 1;
            var signY = Random.Range(0, 2) * 2 - 1;

            return new Vector2(x * signX, y * signY);
        }
    }
}
