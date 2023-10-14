using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;
using Vector2Aether = tainicom.Aether.Physics2D.Common.Vector2;

namespace WrongHole.Objects
{
    public class PlayerBall : Ball
    {
        protected Texture2D _cueTexture;

        protected float _rotation;

        protected MouseState _mouseState;

        protected MouseState _prevMouseState;

        public PlayerBall(float radius, Body body, Color color) : base(radius, body, color)
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _cueTexture = content.Load<Texture2D>("cue");
            base.LoadContent(content);
        }

        public override void Update(GameTime gameTime)
        {
            _prevMouseState = _mouseState;
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                _body.LinearVelocity = Vector2Aether.Zero;

                Vector2Aether rotation = new Vector2Aether(_mouseState.X, _mouseState.Y) - _body.Position;
                _rotation = -(float)Math.Atan2(rotation.X, rotation.Y);
            }

            if (_body.LinearVelocity.Length() != 0) _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer > 0.03)
            {
                ++_animationFrame;
                _animationFrame %= 3;
                _animationTimer = 0;
            }

            if (_prevMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released)
            {
                _body.LinearVelocity = (_body.Position - new Vector2Aether(_mouseState.Position.X, _mouseState.Position.Y)) / 10;
            }
            // Clear the colliding flag
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(/*_animationFrame * _texture.Width / 3*/ 0, 0, _texture.Width / 3, _texture.Height);
            spriteBatch.Draw(
                _texture,
                new Rectangle(new Point((int)(_body.Position.X - _radius * 2 / 3), (int)(_body.Position.Y - _radius * 2 / 5)), new Point((int)_radius * 2)),
                source,
                Color.Black * .5f,
                0,
                new Vector2(_texture.Width / 3, _texture.Height) / 2,
                SpriteEffects.None,
                0);
            spriteBatch.Draw(
                _texture,
                new Rectangle(new Point((int)_body.Position.X, (int)_body.Position.Y), new Point((int)_radius * 2)),
                source,
                _color,
                0,
                new Vector2(_texture.Width / 3, _texture.Height) / 2,
                SpriteEffects.None,
                0);
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 v = new Vector2(_mouseState.Position.X - _body.Position.X, _mouseState.Position.Y - _body.Position.Y);
                spriteBatch.Draw(
                _cueTexture,
                new Rectangle(_mouseState.Position, new Point((int)(Math.Abs(v.Length()) / _cueTexture.Height * _cueTexture.Width), (int)Math.Abs(v.Length()))),
                null,
                Color.White,
                _rotation,
                new Vector2(_cueTexture.Width, _cueTexture.Height) / 2,
                SpriteEffects.None,
                0);
            }
        }
    }
}
