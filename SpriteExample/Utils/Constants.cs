using Microsoft.Xna.Framework;

namespace WrongHole.Utils
{
    public static class Constants
    {
        /// <summary>
        /// The width of the game world.
        /// </summary>
        public static int GAME_WIDTH = 704;

        /// <summary>
        /// The height of the game world.
        /// </summary>
        public static int GAME_HEIGHT = 704;

        public static Vector2 GAME_CENTER = new Vector2(352, 352);

        public static Vector2 GAME_FULL = new Vector2(704, 704);

        public static Vector2 GAME_CENTER_LEFT = new Vector2(176, 352);

        public static Vector2 GAME_CENTER_RIGHT = new Vector2(528, 352);

        /// <summary>
        /// The radius of the balls.
        /// </summary>
        public static int BALL_RADIUS = 32;

        /// <summary>
        /// The ball defualt dampening.
        /// </summary>
        public static float BALL_DEFAULT_DAMPENING = 0.1f;

        public static float GAME_WIDTH_LOW = GAME_WIDTH * 0.2f;
        public static float GAME_WIDTH_HIGH = GAME_WIDTH * 0.8f;
        public static float GAME_HEIGHT_LOW = GAME_HEIGHT * 0.3f;
        public static float GAME_HEIGHT_HIGH = GAME_HEIGHT * 0.7f;

        public static Monochrome[] MONOCHROMES =
        {
            // Coffee.
            new Monochrome
            (new Color[]{
                Tools.ColorFromHex("2B131F"),
                Tools.ColorFromHex("392330"),
                Tools.ColorFromHex("4C333D"),
                Tools.ColorFromHex("68494D"),
                Tools.ColorFromHex("845F5D"),
                Tools.ColorFromHex("C8AC88"),
            }),
            // Sunset.
            new Monochrome
            (new Color[]{
                Tools.ColorFromHex("01084F"),
                Tools.ColorFromHex("391954"),
                Tools.ColorFromHex("631E50"),
                Tools.ColorFromHex("852D55"),
                Tools.ColorFromHex("A73C55"),
                Tools.ColorFromHex("FF7954"),
            }),
            // Ocean.
            new Monochrome
            (new Color[]{
                Tools.ColorFromHex("005073"),
                Tools.ColorFromHex("107DAC"),
                Tools.ColorFromHex("169AD3"),
                Tools.ColorFromHex("1AABD5"),
                Tools.ColorFromHex("1EBBD7"),
                Tools.ColorFromHex("71C7EC"),
            }),
            // Forest.
            new Monochrome
            (new Color[]{
                Tools.ColorFromHex("32343E"),
                Tools.ColorFromHex("404C54"),
                Tools.ColorFromHex("446B6A"),
                Tools.ColorFromHex("467C6F"),
                Tools.ColorFromHex("478D73"),
                Tools.ColorFromHex("5BA95D"),
            }),
            // Skin.
            new Monochrome
            (new Color[]{
                Tools.ColorFromHex("6D6875"),
                Tools.ColorFromHex("b5838d"),
                Tools.ColorFromHex("e5989b"),
                Tools.ColorFromHex("F2A69F"),
                Tools.ColorFromHex("ffb4a2"),
                Tools.ColorFromHex("ffcdb2"),
            }),
            // Violet.
            new Monochrome
            (new Color[]{
                Tools.ColorFromHex("13132B"),
                Tools.ColorFromHex("222238"),
                Tools.ColorFromHex("33334D"),
                Tools.ColorFromHex("484869"),
                Tools.ColorFromHex("5D5D85"),
                Tools.ColorFromHex("8787C7"),
            }),
        };

        public static string SCORE_PATH = "score.txt";

        public static string TILEMAP_PATH = "Levels/";

        public static string TEXTURE_PATH = "Textures/";
        public static string SOUND_PATH = "Sounds/";

        public static string CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+?";

        public static double[] TITLE_CURVE = new double[]
        {
            200,
            150,
            120,
            110,
            80,
            70,
            50,
            30,
            30,
            30,
            10,
            10,
            30,
            30,
            30,
            50,
            70,
            80,
            110,
            120,
            150,
            200,
            5000
        };
    }
}
