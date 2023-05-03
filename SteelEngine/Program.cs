using OpenTK;

namespace SteelEngine
{
    public class SteelEngine
    {
        public static void Main(string[] args)
        {
            EngineProperties properties = new()
            {
                width = 1250,
                height = 740,
                title = "Steel Engine",
                version = "1.0"
            };

            new Engine(properties);
        }
    }
}