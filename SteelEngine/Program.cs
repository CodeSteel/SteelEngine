using SteelEngine.Lua;

namespace SteelEngine
{
    public class SteelEngine
    {
        public static void Main(string[] args)
        {
            EngineProperties properties = new()
            {
                Width = 1250,
                Height = 740,
                BackgroundColor = new Color(0, 0, 0),
                Title = "Steel Engine",
                Version = "1.0"
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