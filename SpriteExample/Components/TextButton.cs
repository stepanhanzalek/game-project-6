using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using WrongHole.Utils;

namespace WrongHole.Components
{
    public class TextButton : Button
    {
        protected SpriteFont _font;

        protected string _text;

        protected Color _colorText;

        public TextButton(ContentManager content, Point center, string text, string textureName, SpriteFont font, Color color, Color colorSelected, Color colorText, bool active = true) : base(content, center, Point.Zero, textureName, color, colorSelected, active)
        {
            _font = font;
            _text = text;
            _colorText = colorText;

            var size = Tools.ScalePoint(Tools.ToPoint(_font.MeasureString(_text)), 1.25f);
            _destRect = new Rectangle(center - Tools.ScalePoint(size, .5f), size);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            var pos = Tools.ToXnaVector(_destRect.Center) - _font.MeasureString(_text) / 2;
            spriteBatch.DrawString(_font, _text, pos, _colorText);
        }
    }
}
