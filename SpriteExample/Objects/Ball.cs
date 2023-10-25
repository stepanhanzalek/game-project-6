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
        public Category Shape;
        protected double _animationTimer;
        protected short _animationFrame = 1;
        protected Texture2D _texture;
        protected float _radius;
        protected Color _color;
        protected Color _colorActive;
        private SoundEffect hit;
        private string _textureName;

        public Ball(float radius, Body body, Color color, Color colorActive, string textureName, Category shape)
        {
            this._radius = radius;
            this._body = body;
            this._body.OnCollision += CollisionHandler;
            this._color = color;
            this._colorActive = colorActive;
            this._textureName = textureName;
            Shape = shape;
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
            _texture = contentManager.Load<Texture2D>(this._textureName);
            hit = contentManager.Load<SoundEffect>("hitHurt");
        }

        public virtual void Update(GameTime gameTime)
        {
            // Clear the colliding flag
            Colliding = false;
        }

        public virtual void Draw(SpriteBatch spriteBatch, bool isActive)
        {
            var source = new Rectangle(0, 0, _texture.Width, _texture.Height);
            spriteBatch.Draw(
                _texture,
                new Rectangle(new Point((int)_body.Position.X, (int)_body.Position.Y), new Point((int)_radius)),
                source,
                isActive ? _colorActive : _color,
                this._body.Rotation,
                new Vector2(_texture.Width, _texture.Height) / 2,
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
