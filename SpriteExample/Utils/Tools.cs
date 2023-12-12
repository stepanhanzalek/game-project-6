using System.Globalization;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2Aether = tainicom.Aether.Physics2D.Common.Vector2;
using Vector2Xna = Microsoft.Xna.Framework.Vector2;

namespace WrongHole.Utils
{
    /// <summary>
    /// A static class providing tools.
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// Converts the Xna vector to an Aether vector.
        /// </summary>
        /// <param name="v">The Xna vector.</param>
        /// <returns>Then Aether vector.</returns>
        public static Vector2Aether ToAetherVector(Vector2Xna v)
        {
            return new Vector2Aether(v.X, v.Y);
        }

        /// <summary>
        /// Converts the Aether vector to an Xna vector.
        /// </summary>
        /// <param name="v">The Aether vector.</param>
        /// <returns>The Xna vector.</returns>
        public static Vector2Xna ToXnaVector(Vector2Aether v)
        {
            return new Vector2Xna(v.X, v.Y);
        }

        /// <summary>
        /// Converts the Point to an Xna vector.
        /// </summary>
        /// <param name="p">The Point.</param>
        /// <returns>The Xna vector.</returns>
        public static Vector2Xna ToXnaVector(Point p)
        {
            return new Vector2Xna(p.X, p.Y);
        }

        /// <summary>
        /// Converts the Xna vector to a Point.
        /// </summary>
        /// <param name="v">The Xna vector.</param>
        /// <returns>The Point.</returns>
        public static Point ToPoint(Vector2Xna v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        /// <summary>
        /// Scales the Point.
        /// </summary>
        /// <param name="p">The Point.</param>
        /// <param name="s">The scale multiplier.</param>
        /// <returns>The scaled Point.</returns>
        public static Point ScalePoint(Point p, float s)
        {
            return new Point((int)(p.X * s), (int)(p.Y * s));
        }

        /// <summary>
        /// Returns points of centers of the vertically aligned buttons.
        /// </summary>
        /// <param name="buttonTexts">The texts of the buttons.</param>
        /// <param name="font">The font.</param>
        /// <returns>The centers of the buttons.</returns>
        public static Point[] AlignButtonsHorizontally(string[] buttonTexts, SpriteFont font)
        {
            Point[] centers = new Point[buttonTexts.Length];

            float width = 0f;

            foreach (var buttonText in buttonTexts) width += font.MeasureString(buttonText).X * 1.25f;

            width += (buttonTexts.Length - 1) * 15;

            Point left = new Point(
                (int)(Constants.GAME_CENTER.X - width / 2),
                (int)Constants.GAME_CENTER.Y);

            for (int i = 0; i < buttonTexts.Length; ++i)
            {
                float textWidth = font.MeasureString(buttonTexts[i]).X;
                centers[i] = left + new Point((int)(textWidth * 1.25f / 2), 0);
                left += new Point((int)(textWidth * 1.25f + 15), 0);
            }
            return centers;
        }

        /// <summary>
        /// Returns points of centers of the vertically aligned buttons.
        /// </summary>
        /// <param name="buttonTexts">The texts of the buttons.</param>
        /// <param name="font">The font.</param>
        /// <returns>The centers of the buttons.</returns>
        public static Point[] AlignButtonsColumn(string[] buttonTexts, SpriteFont font, int columns)
        {
            var rowCenters = AlignButtonsHorizontally(buttonTexts.Take(columns).ToArray(), font);

            int rows = buttonTexts.Length / columns;

            var height = rows * font.MeasureString(buttonTexts[0]).Y * 1.25f;

            int up = (int)(Constants.GAME_CENTER.Y - height / 2);

            var centers = new Point[buttonTexts.Length];

            for (int i = 0; i < buttonTexts.Length; ++i)
            {
                centers[i] = new Point(rowCenters[i % columns].X, up);
                if ((i + 1) % columns == 0 && i != 1) up += (int)(font.MeasureString(buttonTexts[0]).Y * 1.25f) + 15;
            }

            return centers;
        }

        /// <summary>
        /// Converts a color from HEX to RGB.
        /// </summary>
        /// <param name="hex">The HEX color.</param>
        /// <returns>The RGB color.</returns>
        public static Color ColorFromHex(string hex)
        {
            return new Microsoft.Xna.Framework.Color(
                int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber),
                int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber),
                int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber));
        }
    }
}
