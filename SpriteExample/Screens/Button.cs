using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WrongHole.Screens
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
            this.Active = active;
            this._destRect = new Rectangle(center - Tools.ScalePoint(size, .5f), size);
            this._texture = content.Load<Texture2D>(textureName);
            this._colorIdle = color;
            this._colorSelected = colorSelected;
        }

        public event EventHandler ButtonEventHandler;

        public virtual void Update(MouseState mouseLast, MouseState mouseCurr)
        {
            if (!this.Active)
            {
                this._colorActive = new Color(this._colorActive, .5f);
                return;
            }

            (var pos, var stateLast, var stateCurr) = (mouseCurr.Position, mouseLast.LeftButton, mouseCurr.LeftButton);

            if (IsInButton(pos))
            {
                this._colorActive = this._colorSelected;
                if (stateLast == ButtonState.Pressed || stateCurr == ButtonState.Pressed) OnSelect();
                return;
            }
            this._colorActive = this._colorIdle;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                this._texture,
                this._destRect,
                this._colorActive);
        }

        protected internal virtual void OnSelect()
        {
            ButtonEventHandler?.Invoke(this, null);
        }

        protected bool IsInButton(Point position)
        {
            return position.X >= this._destRect.Left
                   && position.Y >= this._destRect.Top
                   && position.X <= this._destRect.Right
                   && position.Y <= this._destRect.Bottom;
        }
    }
}
