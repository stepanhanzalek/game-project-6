using System;

namespace SpriteExample.Content
{
    public static class CollisionHelper
    {
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a._radius + b._radius, 2) >=
                Math.Pow(a._center.X - b._center.X, 2) +
                Math.Pow(a._center.Y - b._center.Y, 2);
        }
    }
}
