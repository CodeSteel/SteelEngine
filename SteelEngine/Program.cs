using SteelEngine.Lua;

namespace SteelEngine
{
    public class SteelEngine
    {
        public static void Main(string[] args)
        {
            EngineProperties properties = new()
            {
                Width = 1240,
                Height = 720,
                BackgroundColor = new Color(0, 0, 0),
                Title = "Steel Engine",
                Version = "1.1"
            };

            string gamePath = ".";
            if (args.Length > 0 && Directory.Exists(args[0]))
                gamePath = args[0];
            else {
                Console.WriteLine("Invalid game directory provided! Running with no-game!");
            }

            new Engine(properties, gamePath);
        }
    }
}