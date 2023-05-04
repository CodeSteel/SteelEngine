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

            new Engine(properties);
        }
    }
}