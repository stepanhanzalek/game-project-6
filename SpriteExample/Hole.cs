using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpriteExample.Content
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
            this._texture = content.Load<Texture2D>(textureName);
            this._position = new Vector2(
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Width * .75f,
                (float)rnd.NextDouble() * graphicsDevice.Viewport.Height * .75f
                );
            this._size = new Point((int)(this._texture.Width * size), (int)(this._texture.Height * size));
            this.bounds = new BoundingCircle(this._position /*+ new Vector2(this._size.X / 2, this._size.Y / 2)*/, this._size.Y / 2);
        }

        public void LoadContent(GraphicsDevice graphicsDevice, ContentManager content, string textureName, Vector2 position, float size = .3f)
        {
            Random rnd = new Random();
            this._texture = content.Load<Texture2D>(textureName);
            this._position = position;
            this._size = new Point((int)(this._texture.Width * size), (int)(this._texture.Height * size));
            this.bounds = new BoundingCircle(this._position /*+ new Vector2(this._size.X / 2, this._size.Y / 2)*/, this._size.Y / 2);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var source = new Rectangle(0, 0, this._texture.Width, this._texture.Height);
            spriteBatch.Draw(
                this._texture,
                new Rectangle(new Point((int)(this._position.X), (int)(this._position.Y)), this._size),
                source,
                Color.Black,
                0,
                new Vector2(this._texture.Width, this._texture.Height) / 2,
                SpriteEffects.None,
                0);
        }
    }
}
