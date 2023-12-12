using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Collision.Shapes;
using tainicom.Aether.Physics2D.Common;
using tainicom.Aether.Physics2D.Dynamics;
using WrongHole.Objects;
using Vector2 = tainicom.Aether.Physics2D.Common.Vector2;

namespace WrongHole.Utils
{
    public static class ObjectFactory
    {
        public static Tuple<int, Ball> GetCircleBall(World world, int radius, Color color, Color colorActive, Vector2 position)
        {
            var body = world.CreateCircle(radius / 2, 1, position, BodyType.Dynamic);
            body.SetRestitution(1);
            body.SetCollisionCategories(Category.Cat1);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Tuple<int, Ball>(body.GetHashCode(), new Ball(radius, body, color, colorActive, "circle", Category.Cat1));
        }

        public static Tuple<int, Ball> GetSquareBall(World world, int radius, Color color, Color colorActive, Vector2 position)
        {
            var body = world.CreateRectangle(radius, radius, 1, position, 0, BodyType.Dynamic);
            body.SetRestitution(1);
            body.SetCollisionCategories(Category.Cat2);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Tuple<int, Ball>(body.GetHashCode(), new Ball(radius, body, color, colorActive, "square", Category.Cat2));
        }

        public static Hole GetHole(World world, Random random, Point size, Color color, Vector2 position)
        {
            var body = world.CreateEllipse(size.X / 2, size.Y / 2, 12, 0, position, 0, BodyType.Static);
            body.SetCollisionCategories(Category.Cat1);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Hole(Tools.ToXnaVector(position), "ball", color, new Point(size.X, size.Y), body, Category.Cat1, 1);
        }

        public static Hole GetCircleHole(World world, Point size, Color color, Vector2 position, int lifespan = 1)
        {
            var body = world.CreateEllipse(size.X / 6, size.Y / 6, 12, 0, position, 0, BodyType.Static);
            body.SetCollisionCategories(Category.Cat1);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Hole(Tools.ToXnaVector(position), "circle", color, new Point(size.X, size.Y), body, Category.Cat1, lifespan);
        }

        public static Hole GetSquareHole(World world, Point size, Color color, Vector2 position, int lifespan = 1)
        {
            var body = world.CreateEllipse(size.X / 6, size.Y / 6, 12, 0, position, 0, BodyType.Static);
            body.SetCollisionCategories(Category.Cat2);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Hole(Tools.ToXnaVector(position), "square", color, new Point(size.X, size.Y), body, Category.Cat2, lifespan);
        }

        public static PolygonShape GetTileMapBox(Vector4[] borders)
        {
            HashSet<Vector2> points = new HashSet<Vector2>();
            foreach (var border in borders)
            {
                points.Add(new Vector2(border.X, border.Y));
                points.Add(new Vector2(border.Z, border.W));
            }
            Vertices verts = new Vertices(points);
            return new PolygonShape(new Vertices(points), 0);
        }

        private static Vector2 GetRandomPos(Random random, Point size)
        {
            return new Vector2(random.Next((int)Constants.GAME_WIDTH_LOW + size.X * 2, (int)Constants.GAME_WIDTH_HIGH - size.X * 2),
                  random.Next((int)Constants.GAME_HEIGHT_LOW + size.Y * 2, (int)Constants.GAME_HEIGHT_HIGH - size.Y * 2));
        }
    }
}
