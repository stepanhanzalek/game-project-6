using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WrongHole
{
    public class Hole
    {
        public bool dummy = true;
        protected Texture2D _texture;

        protected Vector2 _position;

        protected Point _size;

        private BoundingCircle bounds;

        public BoundingCircle Bounds => bounds;

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, string textureName, float size = .3f)
        {
            Random rnd = new Random();
            _texture = content.Load<Texture2D>(textureName);
            _position = new Vector2(
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Width * .75f,
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Height * .75f
                );
            _size = new Point((int)(_texture.Width * size), (int)(_texture.Height * size));
            bounds = new BoundingCircle(_position /*+ new Vector2(this._size.X / 2, this._size.Y / 2)*/, _size.Y / 2);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, string textureName, Vector2 position, float size = .3f)
        {
            Random rnd = new Random();
            _texture = content.Load<Texture2D>(textureName);
            _position = position;
            _size = new Point((int)(_texture.Width * size), (int)(_texture.Height * size));
            bounds = new BoundingCircle(_position /*+ new Vector2(this._size.X / 2, this._size.Y / 2)*/, _size.Y / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(0, 0, _texture.Width, _texture.Height);
            spriteBatch.Draw(
                _texture,
                new Rectangle(new Point((int)_position.X, (int)_position.Y), _size),
                source,
                Color.Black,
                0,
                new Vector2(_texture.Width, _texture.Height) / 2,
                SpriteEffects.None,
                0);
        }
    }
}
