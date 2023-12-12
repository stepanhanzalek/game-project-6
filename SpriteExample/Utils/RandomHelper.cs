using System;
using Microsoft.Xna.Framework;

namespace WrongHole.Utils
{
    /// <summary>
    /// A static class providing helper methods for generating
    /// random values.
    /// </summary>
    public static class RandomHelper
    {
        private static Random random = new Random();

        /// <summary>
        /// Returns a random integer between 0 and <paramref name="maxValue"/>.
        /// </summary>
        /// <param name="maxValue">The maximum value to return.</param>
        /// <returns>A number between 0 and <paramref name="maxValue"/>.</returns>
        public static int Next(int maxValue) => random.Next(maxValue);

        /// <summary>
        /// Returns a random integer within the specified range.
        /// </summary>
        /// <param name="minValue">The minimum integer to return.</param>
        /// <param name="maxValue">The maximum integer to return.</param>
        /// <returns></returns>
        public static int Next(int minValue, int maxValue) => random.Next(minValue, maxValue);

        /// <summary>
        /// Returns a random number within the specified range.
        /// </summary>
        /// <param name="minValue">The minimum end of the range.</param>
        /// <param name="maxValue">The maximum end of the range.</param>
        /// <returns>A radom float between <paramref name="minValue"/> and <paramref name="maxValue"/>.</returns>
        public static float NextFloat(float minValue, float maxValue)
        {
            return minValue + (float)random.NextDouble() * (maxValue - minValue);
        }

        /// <summary>
        /// Returns a unit vector in a random direction.
        /// </summary>
        /// <returns>A random unit Vector2.</returns>
        public static Vector2 NextDirection()
        {
            float angle = NextFloat(0, MathHelper.TwoPi);
            return new Vector2(MathF.Cos(angle), MathF.Sin(angle));
        }

        /// <summary>
        /// Returns a Vector2 that falls within the supplied <paramref name="bounds"/>.
        /// </summary>
        /// <param name="bounds">A rectangle defining the bounds which should contain the point.</param>
        /// <returns>A Vector2 wihtin the bounds.</returns>
        public static Vector2 RandomPosition(Rectangle bounds)
        {
            return new Vector2(
                NextFloat(bounds.X, bounds.X + bounds.Width),
                NextFloat(bounds.Y, bounds.Y + bounds.Height)
                );
        }

        /// <summary>
        /// Returns a Vector2 that falls within the screen.
        /// </summary>
        /// <returns>A Vector2 wihtin the screen.</returns>
        public static Vector2 RandomScreenPosition()
        {
            return new Vector2(
                NextFloat(0, Constants.GAME_WIDTH),
                NextFloat(0, Constants.GAME_HEIGHT)
                );
        }

        /// <summary>
        /// Returns a random <paramref name="len"/> long string.
        /// </summary>
        /// <param name="len">The length of the string.</param>
        /// <returns>The random string.</returns>
        public static string RandomString(int len)
        {
            string res = "";
            for (int i = 0; i < len; i++) res = string.Concat(res, Constants.CHARS[random.Next(Constants.CHARS.Length)]);
            return res;
        }
    }
}
