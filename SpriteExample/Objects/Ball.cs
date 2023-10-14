using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace WrongHole.Objects
{
    public class Ball
    {
        public Body _body;
        protected double _animationTimer;
        protected short _animationFrame = 1;
        protected Texture2D _texture;
        protected float _radius;
        protected Color _color;
        private SoundEffect hit;

        public Ball(float radius, Body body, Color color)
        {
            this._radius = radius;
            this._body = body;
            this._body.OnCollision += CollisionHandler;
            this._color = color;
        }

        /// <summary>
        /// A boolean indicating if this ball is colliding with another
        /// </summary>
        public bool Colliding { get; protected set; }

        /// <summary>
        /// Loads the ball's texture
        /// </summary>
        /// <param name="contentManager">The content manager to use</param>
        public virtual void LoadContent(ContentManager contentManager)
        {
            _texture = contentManager.Load<Texture2D>("ball");
            hit = contentManager.Load<SoundEffect>("hitHurt");
        }

        public virtual void Update(GameTime gameTime)
        {
            // Clear the colliding flag
            Colliding = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(_animationFrame * _texture.Width / 3, 0, _texture.Width / 3, _texture.Height);
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
        }

        private bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            Colliding = true;
            if (other.CollisionCategories.Equals(Category.Cat1)) hit.Play();
            return true;
        }
    }
}
