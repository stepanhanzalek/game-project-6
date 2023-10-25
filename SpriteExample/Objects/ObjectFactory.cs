using System;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using Vector2 = tainicom.Aether.Physics2D.Common.Vector2;

namespace WrongHole.Objects
{
    public static class ObjectFactory
    {
        public static Tuple<int, Ball> GetBall(World world, Random random, int radius, Color color, Color colorActive, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos ? GetRandomPos(random, radius) : position;
            var body = world.CreateCircle(radius / 2, 0, pos, BodyType.Dynamic);
            body.SetRestitution(1);
            body.SetCollisionCategories(Category.Cat1);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Tuple<int, Ball>(body.GetHashCode(), new Ball(radius, body, color, colorActive, "ball", Category.Cat1));
        }

        public static Tuple<int, Ball> GetCircleBall(World world, Random random, int radius, Color color, Color colorActive, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos ? GetRandomPos(random, radius) : position;
            var body = world.CreateCircle(radius / 2, 1, pos, BodyType.Dynamic);
            body.SetRestitution(1);
            body.SetCollisionCategories(Category.Cat1);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Tuple<int, Ball>(body.GetHashCode(), new Ball(radius, body, color, colorActive, "circle", Category.Cat1));
        }

        public static Tuple<int, Ball> GetSquareBall(World world, Random random, int radius, Color color, Color colorActive, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos ? GetRandomPos(random, radius) : position;
            var body = world.CreateRectangle(radius, radius, 1, pos, 0, BodyType.Dynamic);
            body.SetRestitution(1);
            body.SetCollisionCategories(Category.Cat2);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Tuple<int, Ball>(body.GetHashCode(), new Ball(radius, body, color, colorActive, "square", Category.Cat2));
        }

        [Obsolete]
        public static Tuple<int, PlayerBall> GetPlayerBall(World world, Random random, int radius, Color color, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos ? GetRandomPos(random, radius) : position;
            var body = world.CreateCircle(radius / 2, 1, pos, BodyType.Dynamic);
            body.SetRestitution(1);
            body.SetCollisionCategories(Category.Cat3);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Tuple<int, PlayerBall>(body.GetHashCode(), new PlayerBall());
        }

        public static Hole GetHole(World world, Random random, Point size, Color color, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos ? GetRandomPos(random, size) : position;
            var body = world.CreateEllipse(size.X / 2, size.Y / 2, 12, 0, pos, 0, BodyType.Static);
            body.SetCollisionCategories(Category.Cat1);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Hole(new Microsoft.Xna.Framework.Vector2(pos.X, pos.Y), "ball", color, new Point(size.X, size.Y), body, Category.Cat1);
        }

        public static Hole GetCircleHole(World world, Random random, Point size, Color color, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos ? GetRandomPos(random, size) : position;
            var body = world.CreateEllipse(size.X / 2, size.Y / 2, 12, 0, pos, 0, BodyType.Static);
            body.SetCollisionCategories(Category.Cat1);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Hole(new Microsoft.Xna.Framework.Vector2(pos.X, pos.Y), "circle", color, new Point(size.X, size.Y), body, Category.Cat1);
        }

        public static Hole GetSquareHole(World world, Random random, Point size, Color color, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos ? GetRandomPos(random, size) : position;

            var body = world.CreateEllipse(size.X / 2, size.Y / 2, 12, 0, pos, 0, BodyType.Static);
            body.SetCollisionCategories(Category.Cat2);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Hole(new Microsoft.Xna.Framework.Vector2(pos.X, pos.Y), "square", color, new Point(size.X, size.Y), body, Category.Cat2);
        }

        private static Vector2 GetRandomPos(Random random, Point size)
        {
            return new Vector2(random.Next((int)Constants.GAME_WIDTH_LOW + size.X * 2, (int)Constants.GAME_WIDTH_HIGH - size.X * 2),
                  random.Next((int)Constants.GAME_HEIGHT_LOW + size.Y * 2, (int)Constants.GAME_HEIGHT_HIGH - size.Y * 2));
        }

        private static Vector2 GetRandomPos(Random random, int size)
        {
            return new Vector2(random.Next((int)Constants.GAME_WIDTH_LOW + size * 2, (int)Constants.GAME_WIDTH_HIGH - size * 2),
                  random.Next((int)Constants.GAME_HEIGHT_LOW + size * 2, (int)Constants.GAME_HEIGHT_HIGH - size * 2));
        }
    }
}
