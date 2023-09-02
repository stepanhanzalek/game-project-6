using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpriteExample.Content
{
    public class Ball
    {
        protected Texture2D _texture;

        protected Texture2D _cueTexture;

        protected float _rotation;

        protected Vector2 _position;

        protected MouseState _mouseState;

        protected MouseState _prevMouseState;

        protected Vector2 _velocity;

        protected Vector2 _gravityPull = new Vector2(1.02f, 1.02f);

        protected Point _size;

        private double animationTimer;

        private short animationFrame = 1;

        private BoundingCircle bounds;

        public BoundingCircle Bounds => bounds;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, string textureName, float size = .25f)
        {
            Random rnd = new Random();
            this._texture = content.Load<Texture2D>(textureName);
            this._cueTexture = content.Load<Texture2D>("cue");
            this._position = new Vector2(
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Width * .75f,
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Height * .75f
                );
            this._rotation = 0f;
            this._size = new Point((int)(this._texture.Width * size / 3), (int)(this._texture.Height * size));
            this.bounds = new BoundingCircle(this._position + new Vector2(this._size.X / 2, this._size.Y / 2), this._size.Y / 2);
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            this._prevMouseState = this._mouseState;
            this._mouseState = Mouse.GetState();
            if (this._mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 rotation = new Vector2((float)this._mouseState.X, (float)this._mouseState.Y) - this._position;
                this._rotation = -(float)Math.Atan2(rotation.X, rotation.Y);
            }
            else
            {
                this.resolveCollision(gameTime, graphicsDevice);

                if (this._velocity.Length() != 0) animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.03)
                {
                    ++animationFrame;
                    animationFrame %= 3;
                    animationTimer = 0;
                }
                this.bounds._center = this._position;
            }

            if ((this._prevMouseState.LeftButton == ButtonState.Pressed) && this._mouseState.LeftButton == ButtonState.Released)
            {
                this._velocity = this._position - new Vector2(this._mouseState.Position.X, this._mouseState.Position.Y);
                this._gravityPull = -this._velocity * 0.15f;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(this.animationFrame * this._texture.Width / 3, 0, this._texture.Width / 3, this._texture.Height);
            spriteBatch.Draw(
                this._texture,
                new Rectangle(new Point((int)(this._position.X - this._size.X / 3), (int)(this._position.Y - this._size.Y / 5)), this._size),
                source,
                Color.Black * .5f,
                0,
                new Vector2(this._texture.Width / 3, this._texture.Height) / 2,
                SpriteEffects.None,
                0);
            spriteBatch.Draw(
                this._texture,
                new Rectangle(new Point((int)this._position.X, (int)this._position.Y), this._size),
                source,
                Color.White,
                this._rotation,
                new Vector2(this._texture.Width / 3, this._texture.Height) / 2,
                SpriteEffects.None,
                0);
            if (this._mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 v = new Vector2(this._mouseState.Position.X, this._mouseState.Position.Y) - this._position;
                spriteBatch.Draw(
                this._cueTexture,
                new Rectangle(this._mouseState.Position, new Point((int)((Math.Abs(v.Length()) / this._cueTexture.Height) * this._cueTexture.Width), (int)Math.Abs(v.Length()))),
                null,
                Color.White,
                this._rotation,
                new Vector2(this._cueTexture.Width, this._cueTexture.Height) / 2,
                SpriteEffects.None,
                0);
            }
        }

        protected void resolveCollision(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            this._position += this._velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            this._velocity = applyGravityPull(this._velocity, this._gravityPull * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (this._position.X + this._size.X / 2 >= graphicsDevice.Viewport.Width || this._position.X <= 0)
            {
                this._velocity.X *= -1;
                this._gravityPull.X *= -1;
            }
            if (this._position.Y + this._size.Y / 2 >= graphicsDevice.Viewport.Height || this._position.Y <= 0)
            {
                this._velocity.Y *= -1;
                this._gravityPull.Y *= -1;
            }
        }

        protected Vector2 applyGravityPull(Vector2 speed, Vector2 gravityPull)
        {
            Vector2 product = speed + gravityPull;
            if (Math.Sign(product.X) != Math.Sign(speed.X)) product.X = 0f;
            if (Math.Sign(product.Y) != Math.Sign(speed.Y)) product.Y = 0f;
            return product;
        }
    }
}
