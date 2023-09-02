using Microsoft.Xna.Framework;

namespace SpriteExample.Content
{
    public struct BoundingCircle
    {
        public Vector2 _center;

        public float _radius;

        public BoundingCircle(Vector2 center, float radius)
        {
            _center = center;
            _radius = radius;
        }

        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
