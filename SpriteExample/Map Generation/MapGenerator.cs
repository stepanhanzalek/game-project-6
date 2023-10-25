using System;
using System.Collections.Generic;

namespace WrongHole.MapGeneration
{
    public class MapGenerator
    {
        protected List<Tuple<int, int>> _map;

        protected Random _random;

        protected Direction _direction;

        protected Dictionary<Direction, Tuple<int, int>> _step;

        public MapGenerator()
        {
            for (int i = 0; i < 10; i++)
            {
            }
        }

        protected enum Direction
        {
            Up,
            Right,
            Down,
            Left
        }

        protected void Step()
        {
            int rand = _random.Next(4);
            Direction newDirection = _direction;
            if (rand == 0) ++newDirection;
            else if (rand == 3) --newDirection;

            switch (_direction)
            {
                case Direction.Up:
                    break;
            }
        }
    }
}
