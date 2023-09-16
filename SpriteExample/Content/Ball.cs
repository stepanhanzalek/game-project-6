using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SpriteExample.Content;

namespace SpriteExample
{
    public class Ball
    {
        public float Radius;

        public Vector2 Center;
        public Vector2 Velocity = new Vector2(10, 10);
        public bool Colliding;
        public bool IsColliding;
        protected Texture2D _texture;
        protected float _frictionConstant = 100f;

        protected Point _size;

        protected double animationTimer;

        protected short animationFrame = 1;

        protected BoundingCircle bounds;
        public BoundingCircle Bounds => bounds;

        public virtual void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, string textureName, float size = .25f)
        {
            Random rnd = new Random();
            this._texture = content.Load<Texture2D>(textureName);
            this.Center = new Vector2(
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Width * .75f,
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Height * .75f
                );
            this._size = new Point((int)(this._texture.Width * size / 3), (int)(this._texture.Height * size));
            this.Radius = this._size.X / 2;
            this.bounds = new BoundingCircle(this.Center + new Vector2(this._size.X / 2, this._size.Y / 2), this._size.Y / 2);
        }

        public virtual void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
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
            this.Colliding = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
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
                0,
                new Vector2(this._texture.Width / 3, this._texture.Height) / 2,
                SpriteEffects.None,
                0);
        }

        public virtual bool CollidesWith(Ball ball)
        {
            return this.Radius + ball.Radius >= Vector2.Distance(Center, ball.Center);
        }

        protected void resolveCollision(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            this.Center += this.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (this.Center.X + this._size.X / 2 >= graphicsDevice.Viewport.Width || this.Center.X <= (float)this._size.X / 2)
            {
                this.Velocity.X *= -1;
                this.Velocity *= .8f;
                this.Center.X = this.Center.X + this._size.X / 2 >= graphicsDevice.Viewport.Width ? graphicsDevice.Viewport.Width - this._size.X / 2 : this._size.X / 2;
            }
            if (this.Center.Y + this._size.Y / 2 >= graphicsDevice.Viewport.Height || this.Center.Y <= (float)this._size.X / 2)
            {
                this.Velocity.Y *= -1;
                this.Velocity *= .8f;
                this.Center.Y = this.Center.Y + this._size.Y / 2 >= graphicsDevice.Viewport.Height ? graphicsDevice.Viewport.Height - this._size.Y / 2 : this._size.Y / 2;
            }

            this.Velocity = applyGravityPull(this.Velocity, this._frictionConstant, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected Vector2 applyGravityPull(Vector2 speed, float frictionConstant, float elapsedTime)
        {
            if (speed.LengthSquared() == 0) return speed;

            Vector2 speedDir = new Vector2(speed.X, speed.Y);
            speedDir.Normalize();

            Vector2 slowdownDiff = (speedDir * frictionConstant) * elapsedTime;

            if (slowdownDiff.Length() >= speed.Length())
            {
                return Vector2.Zero;
            }

            return speed - slowdownDiff;
        }
    }
}
