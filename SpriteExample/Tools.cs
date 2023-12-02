using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2Aether = tainicom.Aether.Physics2D.Common.Vector2;
using Vector2Xna = Microsoft.Xna.Framework.Vector2;

namespace WrongHole
{
    public static class Tools
    {
        public static Vector2Aether ToAetherVector(Vector2Xna v)
        {
            return new Vector2Aether(v.X, v.Y);
        }

        public static Vector2Xna ToXnaVector(Vector2Aether v)
        {
            return new Vector2Xna(v.X, v.Y);
        }

        public static Vector2Xna ToXnaVector(Point p)
        {
            return new Vector2Xna(p.X, p.Y);
        }

        public static Point ToPoint(Vector2Xna v)
        {
            return new Point((int)v.X, (int)v.Y);
        }

        public static Point ScalePoint(Point p, float s)
        {
            return new Point((int)(p.X * s), (int)(p.Y * s));
        }

        public static Point[] AlignButtonsHorizontally(string[] buttonTexts, SpriteFont font)
        {
            Point[] centers = new Point[buttonTexts.Length];

            float width = 0f;

            foreach (var buttonText in buttonTexts) width += font.MeasureString(buttonText).X * 1.25f;

            width += (buttonTexts.Length - 1) * 15;

            Point left = new Point(
                (int)(Constants.GAME_CENTER.X - width / 2),
                (int)(Constants.GAME_CENTER.Y));

            for (int i = 0; i < buttonTexts.Length; ++i)
            {
                float textWidth = font.MeasureString(buttonTexts[i]).X;
                centers[i] = left + new Point((int)(textWidth * 1.25f / 2), 0);
                left += new Point((int)(textWidth * 1.25f + 15), 0);
            }
            return centers;
        }
    }
}
