using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WrongHole.Content
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

        public Ball(Vector2 pos)
        {
            Center = pos;
        }

        public BoundingCircle Bounds => bounds;

        public virtual void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, string textureName, float size = .25f)
        {
            _texture = content.Load<Texture2D>(textureName);
            _size = new Point((int)(_texture.Width * size / 3), (int)(_texture.Height * size));
            Radius = _size.X / 2;
            bounds = new BoundingCircle(Center + new Vector2(_size.X / 2, _size.Y / 2), _size.Y / 2);
        }

        public virtual void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
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
            Colliding = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
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
                0,
                new Vector2(_texture.Width / 3, _texture.Height) / 2,
                SpriteEffects.None,
                0);
        }

        public virtual bool CollidesWith(Ball ball)
        {
            return Radius + ball.Radius >= Vector2.Distance(Center, ball.Center);
        }

        protected void resolveCollision(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            Center += Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Center.X + _size.X / 2 >= graphicsDevice.Viewport.Width || Center.X <= (float)_size.X / 2)
            {
                Velocity.X *= -1;
                Velocity *= .8f;
                Center.X = Center.X + _size.X / 2 >= graphicsDevice.Viewport.Width ? graphicsDevice.Viewport.Width - _size.X / 2 : _size.X / 2;
            }
            if (Center.Y + _size.Y / 2 >= graphicsDevice.Viewport.Height || Center.Y <= (float)_size.X / 2)
            {
                Velocity.Y *= -1;
                Velocity *= .8f;
                Center.Y = Center.Y + _size.Y / 2 >= graphicsDevice.Viewport.Height ? graphicsDevice.Viewport.Height - _size.Y / 2 : _size.Y / 2;
            }

            Velocity = applyGravityPull(Velocity, _frictionConstant, (float)gameTime.ElapsedGameTime.TotalSeconds);
        }

        protected Vector2 applyGravityPull(Vector2 speed, float frictionConstant, float elapsedTime)
        {
            if (speed.LengthSquared() == 0) return speed;

            Vector2 speedDir = new Vector2(speed.X, speed.Y);
            speedDir.Normalize();

            Vector2 slowdownDiff = speedDir * frictionConstant * elapsedTime;

            if (slowdownDiff.Length() >= speed.Length())
            {
                return Vector2.Zero;
            }

            return speed - slowdownDiff;
        }
    }
}
