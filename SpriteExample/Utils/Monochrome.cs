using System;
using Microsoft.Xna.Framework;

namespace WrongHole.Utils
{
    public class Monochrome
    {
        private Color[] _colors;

        public Monochrome(Color[] colors)
        {
            if (colors.Length != 6) throw new ArgumentException("Monochrome should have 5 colors.");
            _colors = colors;
        }

        public Color Background
        { get { return _colors[2]; } }

        public Color Wall
        { get { return _colors[1]; } }

        public Color FireworkFail
        { get { return _colors[0]; } }

        public Color FireworkWin
        { get { return _colors[4]; } }

        public Color Ball
        { get { return _colors[4]; } }

        public Color BallActive
        { get { return _colors[5]; } }

        public Color BallPassive
        { get { return _colors[3]; } }

        public Color Hole
        { get { return _colors[0]; } }

        public Color Text
        { get { return _colors[1]; } }

        public Color TextLight
        { get { return _colors[5]; } }

        public Color Button
        { get { return _colors[0]; } }

        public Color ButtonSelected
        { get { return _colors[1]; } }

        public Color ButtonText
        { get { return _colors[4]; } }
    }
}
