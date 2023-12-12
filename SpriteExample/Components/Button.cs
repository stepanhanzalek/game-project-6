using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WrongHole.Utils;

namespace WrongHole.Components
{
    public class Button
    {
        public bool Active;
        protected Rectangle _destRect;

        protected Texture2D _texture;

        protected Color _colorIdle, _colorSelected;

        protected Color _colorActive;

        protected bool _isFocused;

        public Button(ContentManager content, Point center, Point size, string textureName, Color color, Color colorSelected, bool active = true)
        {
            Active = active;
            _destRect = new Rectangle(center - Tools.ScalePoint(size, .5f), size);
            _texture = content.Load<Texture2D>(Constants.TEXTURE_PATH + textureName);
            _colorIdle = color;
            _colorSelected = colorSelected;
        }

        public event EventHandler ButtonEventHandler;

        public virtual void Update(MouseState mouseLast, MouseState mouseCurr)
        {
            if (!Active)
            {
                _colorActive = new Color(_colorActive, .5f);
                return;
            }

            (var pos, var stateLast, var stateCurr) = (mouseCurr.Position, mouseLast.LeftButton, mouseCurr.LeftButton);

            if (IsInButton(pos))
            {
                _colorActive = _colorSelected;
                if (stateLast == ButtonState.Pressed || stateCurr == ButtonState.Pressed) OnSelect();
                return;
            }
            _colorActive = _colorIdle;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                _texture,
                _destRect,
                _colorActive);
        }

        protected internal virtual void OnSelect()
        {
            ButtonEventHandler?.Invoke(this, null);
        }

        protected bool IsInButton(Point position)
        {
            return position.X >= _destRect.Left
                   && position.Y >= _destRect.Top
                   && position.X <= _destRect.Right
                   && position.Y <= _destRect.Bottom;
        }
    }
}
