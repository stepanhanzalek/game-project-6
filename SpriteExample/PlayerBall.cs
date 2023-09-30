using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using WrongHole.Content;

namespace WrongHole
{
    public class PlayerBall : Ball
    {
        protected Texture2D _cueTexture;

        protected float _rotation;

        protected MouseState _mouseState;

        protected MouseState _prevMouseState;

        public PlayerBall() : base(Vector2.Zero)
        {
        }

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, string textureName, float size = .25f)
        {
            Random rnd = new Random();
            _texture = content.Load<Texture2D>(textureName);
            _cueTexture = content.Load<Texture2D>("cue");
            Center = new Vector2(
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Width * .75f,
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Height * .75f
                );
            _rotation = 0f;
            _size = new Point((int)(_texture.Width * size / 3), (int)(_texture.Height * size));
            Radius = _size.X / 2;
            bounds = new BoundingCircle(Center + new Vector2(_size.X / 2, _size.Y / 2), _size.Y / 2);
        }

        public override void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            _prevMouseState = _mouseState;
            _mouseState = Mouse.GetState();
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 rotation = new Vector2(_mouseState.X, _mouseState.Y) - Center;
                _rotation = -(float)Math.Atan2(rotation.X, rotation.Y);
            }
            else
            {
                resolveCollision(gameTime, graphicsDevice);

                if (Velocity.Length() != 0) animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.03)
                {
                    ++animationFrame;
                    animationFrame %= 3;
                    animationTimer = 0;
                }
                bounds._center = Center;
            }

            if (_prevMouseState.LeftButton == ButtonState.Pressed && _mouseState.LeftButton == ButtonState.Released)
            {
                Velocity = Center - new Vector2(_mouseState.Position.X, _mouseState.Position.Y);
            }

            Colliding = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(animationFrame * _texture.Width / 3, 0, _texture.Width / 3, _texture.Height);
            if (Colliding) spriteBatch.Draw(
                _texture,
                new Rectangle(new Point((int)(Center.X - _size.X / 3), (int)(Center.Y - _size.Y / 5)), _size),
                source,
                Color.Black * .5f,
                0,
                new Vector2(_texture.Width / 3, _texture.Height) / 2,
                SpriteEffects.None,
                0);
            spriteBatch.Draw(
                _texture,
                new Rectangle(new Point((int)Center.X, (int)Center.Y), _size),
                source,
                Color.White,
                _rotation,
                new Vector2(_texture.Width / 3, _texture.Height) / 2,
                SpriteEffects.None,
                0);
            if (_mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 v = new Vector2(_mouseState.Position.X, _mouseState.Position.Y) - Center;
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
