using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WrongHole.Screens
{
    public class TextButton : Button
    {
        protected SpriteFont _font;

        protected string _text;

        protected Color _colorText;

        public TextButton(ContentManager content, Point center, string text, string textureName, SpriteFont font, Color color, Color colorSelected, Color colorText, bool active = true) : base(content, center, Point.Zero, textureName, color, colorSelected, active)
        {
            this._font = font;
            this._text = text;
            this._colorText = colorText;

            var size = Tools.ScalePoint(Tools.ToPoint(this._font.MeasureString(this._text)), 1.25f);
            this._destRect = new Rectangle(center - Tools.ScalePoint(size, .5f), size);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            var pos = Tools.ToXnaVector(this._destRect.Center) - this._font.MeasureString(this._text) / 2;
            spriteBatch.DrawString(this._font, this._text, pos, this._colorText);
        }
    }
}
