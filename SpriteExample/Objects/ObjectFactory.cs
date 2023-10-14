using System;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D.Dynamics;
using Vector2 = tainicom.Aether.Physics2D.Common.Vector2;

namespace WrongHole.Objects
{
    public static class ObjectFactory
    {
        public static Tuple<int, Ball> GetBall(World world, Random random, int radius, Color color, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos
                ? new Vector2(random.Next((int)Constants.GAME_WIDTH_LOW + radius, (int)Constants.GAME_WIDTH_HIGH - radius),
                  random.Next((int)Constants.GAME_HEIGHT_LOW + radius, (int)Constants.GAME_HEIGHT_HIGH - radius))
                : position;
            var body = world.CreateCircle(radius, 0, pos, BodyType.Dynamic);
            body.SetRestitution(1);
            body.SetCollisionCategories(Category.Cat1);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Tuple<int, Ball>(body.GetHashCode(), new Ball(radius, body, color));
        }

        public static Tuple<int, PlayerBall> GetPlayerBall(World world, Random random, int radius, Color color, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos
                ? new Vector2(random.Next((int)Constants.GAME_WIDTH_LOW + radius, (int)Constants.GAME_WIDTH_HIGH - radius),
                  random.Next((int)Constants.GAME_HEIGHT_LOW + radius, (int)Constants.GAME_HEIGHT_HIGH - radius))
                : position;
            var body = world.CreateCircle(radius, 0, pos, BodyType.Dynamic);
            body.SetRestitution(1);
            body.SetCollisionCategories(Category.Cat1);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Tuple<int, PlayerBall>(body.GetHashCode(), new PlayerBall(radius, body, color));
        }

        public static Hole GetHole(World world, Random random, Point size, Color color, bool randomPos = false, Vector2 position = default)
        {
            var pos = randomPos
                ? new Vector2(random.Next((int)Constants.GAME_WIDTH_LOW + size.X * 2, (int)Constants.GAME_WIDTH_HIGH - size.X * 2),
                  random.Next((int)Constants.GAME_HEIGHT_LOW + size.Y * 2, (int)Constants.GAME_HEIGHT_HIGH - size.Y * 2))
                : position;
            var body = world.CreateEllipse(size.X, size.Y, 12, 0, pos, 0, BodyType.Static);
            body.SetCollisionCategories(Category.Cat2);
            body.LinearDamping = Constants.BALL_DEFAULT_DAMPENING;
            return new Hole(new Microsoft.Xna.Framework.Vector2(pos.X, pos.Y), "ball", color, new Point(2 * size.X, 2 * size.Y), body);
        }
    }
}
