using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2Aether = tainicom.Aether.Physics2D.Common.Vector2;

namespace WrongHole.Objects
{
    public class PlayerBall
    {
        protected Texture2D _cueTexture;

        protected float _rotation;

        protected MouseState _mouseState;

        protected MouseState _prevMouseState;

        protected Color _color;

        public PlayerBall()
        {
        }

        public void LoadContent(ContentManager content, Color color)
        {
            _cueTexture = content.Load<Texture2D>("cue");
            _color = color;
        }

        public void Update(GameTime gameTime, ref Ball ball)
        {
            _prevMouseState = _mouseState;
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                ball._body.LinearVelocity = Vector2Aether.Zero;

                Vector2Aether rotation = new Vector2Aether(_mouseState.X, _mouseState.Y) - ball._body.Position;
                _rotation = -(float)Math.Atan2(rotation.X, rotation.Y);
            }

            if (_prevMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released)
            {
                ball._body.LinearVelocity = (ball._body.Position - new Vector2Aether(_mouseState.Position.X, _mouseState.Position.Y)) / 10;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2Aether ballPos)
        {
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 v = new Vector2(_mouseState.Position.X - ballPos.X, _mouseState.Position.Y - ballPos.Y);
                spriteBatch.Draw(
                _cueTexture,
                new Rectangle(_mouseState.Position, new Point((int)(Math.Abs(v.Length()) / _cueTexture.Height * _cueTexture.Width), (int)Math.Abs(v.Length()))),
                null,
                _color,
                _rotation,
                new Vector2(_cueTexture.Width, _cueTexture.Height) / 2,
                SpriteEffects.None,
                0);
            }
        }
    }
}
