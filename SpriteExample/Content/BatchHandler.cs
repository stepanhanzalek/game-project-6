using Microsoft.Xna.Framework.Graphics;

namespace WrongHole.Content
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
