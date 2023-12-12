using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using WrongHole.StateManagement;
using WrongHole.Utils;

namespace WrongHole.Screens
{
    public class ColorPalleteScreen : TileMapLevelScreen
    {
        protected Queue<Monochrome> _monochromes;

        public ColorPalleteScreen(int currLvl) : base(currLvl)
        {
            _monochromes = new Queue<Monochrome>(Constants.MONOCHROMES);

            _monochrome = _monochromes.Dequeue();
        }

        public override void HandleInput(GameTime gameTime, InputState input)
        {
            base.HandleInput(gameTime, input);

            if (_lastState.IsKeyDown(Keys.Tab) && !_currState.IsKeyDown(Keys.Tab))
            {
                if (_monochromes.Count == 0) _monochromes = new Queue<Monochrome>(Constants.MONOCHROMES);
                _monochrome = _monochromes.Dequeue();
                this.Activate();
            }
        }
    }
}
