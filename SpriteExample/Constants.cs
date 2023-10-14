using Microsoft.Xna.Framework;

namespace WrongHole
{
    public static class Constants
    {
        /// <summary>
        /// The width of the game world.
        /// </summary>
        public static int GAME_WIDTH = 760;

        /// <summary>
        /// The height of the game world.
        /// </summary>
        public static int GAME_HEIGHT = 480;

        /// <summary>
        /// The radius of the balls.
        /// </summary>
        public static int BALL_RADIUS = 15;

        /// <summary>
        /// The ball defualt dampening.
        /// </summary>
        public static float BALL_DEFAULT_DAMPENING = 0.1f;

        public static float GAME_WIDTH_LOW = GAME_WIDTH * 0.2f;
        public static float GAME_WIDTH_HIGH = GAME_WIDTH * 0.8f;
        public static float GAME_HEIGHT_LOW = GAME_HEIGHT * 0.3f;
        public static float GAME_HEIGHT_HIGH = GAME_HEIGHT * 0.7f;

        public static Color[][] MONOCHROMES =
        {
            new Color[]
            {
                new Color(1, 8, 79),
                new Color(57,25,84),
                new Color(99,30,80),
                new Color(167,60,90),
                new Color(255,121,84),
            },
            new Color[]
            {
                new Color(0,80,115),
                new Color(16,125,172),
                new Color(24,154,211),
                new Color(30,187,215),
                new Color(113,199,236),
            },
            new Color[]
            {
                new Color(50,52,62),
                new Color(64,76,84),
                new Color(68,107,106),
                new Color(71,141,115),
                new Color(91,169,93),
            },
        };
    }
}
