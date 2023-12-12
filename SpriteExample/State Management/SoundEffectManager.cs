using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using WrongHole.Utils;

namespace WrongHole.StateManagement
{
    public class SoundEffectManager
    {
        public SoundEffectManager()
        {
        }

        public SoundEffect Collision { get; private set; }
        public SoundEffect Win { get; private set; }
        public SoundEffect Explosion { get; private set; }

        public void LoadContent(ContentManager content)
        {
            Collision = content.Load<SoundEffect>(Constants.SOUND_PATH + "hitHurt");
            Win = content.Load<SoundEffect>(Constants.SOUND_PATH + "pickupCoin");
            Explosion = content.Load<SoundEffect>(Constants.SOUND_PATH + "explosion");
        }
    }
}
