using System;
using Microsoft.Xna.Framework;

namespace WrongHole
{
    public static class CollisionHelper
    {
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a._radius + b._radius, 2) >=
                Math.Pow(a._center.X - b._center.X, 2) +
                Math.Pow(a._center.Y - b._center.Y, 2);
        }

        public static bool Collides(float radius, Vector2 a, Vector2 b)
        {
            return Math.Pow(radius + radius, 2) >=
                Math.Pow(b.X - b.X, 2) +
                Math.Pow(a.Y - b.Y, 2);
        }
    }
}
