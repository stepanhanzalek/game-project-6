using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpriteExample.Content;

namespace SpriteExample
{
    public class PlayerBall : Ball
    {
        protected Texture2D _cueTexture;

        protected float _rotation;

        protected MouseState _mouseState;

        protected MouseState _prevMouseState;

        public override void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, string textureName, float size = .25f)
        {
            Random rnd = new Random();
            this._texture = content.Load<Texture2D>(textureName);
            this._cueTexture = content.Load<Texture2D>("cue");
            this.Center = new Vector2(
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Width * .75f,
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Height * .75f
                );
            this._rotation = 0f;
            this._size = new Point((int)(this._texture.Width * size / 3), (int)(this._texture.Height * size));
            this.Radius = this._size.X / 2;
            this.bounds = new BoundingCircle(this.Center + new Vector2(this._size.X / 2, this._size.Y / 2), this._size.Y / 2);
        }

        public override void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            this._prevMouseState = this._mouseState;
            this._mouseState = Mouse.GetState();
            if (this._mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 rotation = new Vector2((float)this._mouseState.X, (float)this._mouseState.Y) - this.Center;
                this._rotation = -(float)Math.Atan2(rotation.X, rotation.Y);
            }
            else
            {
                this.resolveCollision(gameTime, graphicsDevice);

                if (this.Velocity.Length() != 0) animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
                if (animationTimer > 0.03)
                {
                    ++animationFrame;
                    animationFrame %= 3;
                    animationTimer = 0;
                }
                this.bounds._center = this.Center;
            }

            if ((this._prevMouseState.LeftButton == ButtonState.Pressed) && this._mouseState.LeftButton == ButtonState.Released)
            {
                this.Velocity = this.Center - new Vector2(this._mouseState.Position.X, this._mouseState.Position.Y);
            }

            this.Colliding = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(this.animationFrame * this._texture.Width / 3, 0, this._texture.Width / 3, this._texture.Height);
            if (this.Colliding) spriteBatch.Draw(
                this._texture,
                new Rectangle(new Point((int)(this.Center.X - this._size.X / 3), (int)(this.Center.Y - this._size.Y / 5)), this._size),
                source,
                Color.Black * .5f,
                0,
                new Vector2(this._texture.Width / 3, this._texture.Height) / 2,
                SpriteEffects.None,
                0);
            spriteBatch.Draw(
                this._texture,
                new Rectangle(new Point((int)this.Center.X, (int)this.Center.Y), this._size),
                source,
                Color.White,
                this._rotation,
                new Vector2(this._texture.Width / 3, this._texture.Height) / 2,
                SpriteEffects.None,
                0);
            if (this._mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 v = new Vector2(this._mouseState.Position.X, this._mouseState.Position.Y) - this.Center;
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
    }
}
