using Microsoft.Xna.Framework.Graphics;

namespace SpriteExample.Content
{
    public class BatchHandler
    {
        protected SpriteBatch spriteBatch;

        public void LoadContent(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }
    }
}
