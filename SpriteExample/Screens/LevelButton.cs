using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WrongHole.Screens
{
    public class LevelButton : TextButton
    {
        public int Level;

        public LevelButton(ContentManager content, Point center, int level, string text, string textureName, SpriteFont font, Color color, Color colorSelected, Color colorText, bool active = true) : base(content, center, text, textureName, font, color, colorSelected, colorText, active)
        {
            this.Level = level;
        }
    }
}
