using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Dynamics.Contacts;

namespace WrongHole.Objects
{
    public class Hole
    {
        public bool dummy = true;
        public int CollisionHash;
        public Category Shape;
        public int Lifespan;
        public Body _body;
        protected string _textureName;

        protected Texture2D _texture;

        protected Vector2 _position;

        protected Point _size;

        private Color _color;

        public Hole(Vector2 position, string textureName, Color color, Point size, Body body, Category shape, int lifespan)
        {
            _position = position;
            _textureName = textureName;
            _color = color;
            _size = size;
            _body = body;
            _body.OnCollision += CollisionHandler;
            Shape = shape;
            Lifespan = lifespan;
        }

        /// <summary>
        /// A boolean indicating if this ball is colliding with another
        /// </summary>
        public bool Colliding { get; protected set; }

        public void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>(_textureName);
        }

        public virtual void Update(GameTime gameTime)
        {
            // Clear the colliding flag
            Colliding = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(0, 0, _texture.Width, _texture.Height);
            spriteBatch.Draw(
                _texture,
                new Rectangle(new Point((int)_body.Position.X - _size.X / 2, (int)_body.Position.Y - _size.Y / 2), _size),
                source,
                _color,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                0);
        }

        private bool CollisionHandler(Fixture fixture, Fixture other, Contact contact)
        {
            CollisionHash = other.Body.GetHashCode();
            Colliding = true;
            return false;
        }
    }
}
