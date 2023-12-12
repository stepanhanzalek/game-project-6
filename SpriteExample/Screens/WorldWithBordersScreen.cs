using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;
using WrongHole.StateManagement;
using Vector2 = tainicom.Aether.Physics2D.Common.Vector2;

namespace WrongHole.Screens
{
    public class WorldWithBordersScreen : GameScreen
    {
        protected World _world;
        protected ContentManager _content;
        protected GraphicsDevice _graphics;
        protected Texture2D _blankTexture;

        public WorldWithBordersScreen() : base()
        {
        }

        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, whereas if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");
            _graphics = ScreenManager.GraphicsDevice;

            _blankTexture = new Texture2D(_graphics, 1, 1);
            _blankTexture.SetData(new[] { Color.White });

            _world = new World();
            _world.Gravity = Vector2.Zero;
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // Unlike most screens, this should not transition off even if
        // it has been covered by another screen: it is supposed to be
        // covered, after all! This overload forces the coveredByOtherScreen
        // parameter to false in order to stop the base Update method wanting to transition off.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            for (int i = 0; i < 10; ++i) _world.Step(Math.Min((float)gameTime.ElapsedGameTime.TotalSeconds, 1f / 30f));
            base.Update(gameTime, otherScreenHasFocus, false);
        }
    }
}
