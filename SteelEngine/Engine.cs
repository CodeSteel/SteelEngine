using SteelEngine.Lua;
using System.ComponentModel;

namespace SteelEngine
{
    /// <summary>
    /// This is used to define the 'startup' parameters.
    /// An EngineProperties object is passed in the Steel.Preload function and returned before starting the engine.
    /// </summary>
    public struct EngineProperties
    {
        /// <summary>
        /// The width of the window.
        /// </summary>
        public int Width;

        /// <summary>
        /// The height of the window.
        /// </summary>
        public int Height;
        
        /// <summary>
        /// The title of the window.
        /// </summary>
        public string Title;

        /// <summary>
        /// (optional) The version of the application.
        /// </summary>
        public string? Version;

        /// <summary>
        /// The background color to draw when cleared.
        /// </summary>
        public Color BackgroundColor;
    }

    internal class Engine
    {
        public static Engine instance;

        private Game window;
        public NLua.Lua luaState;

        public Engine(EngineProperties properties, string gameDirectory)
        {
            instance = this;

            // initialize lua
            luaState = new NLua.Lua();

            // check if main.lua exists, otherwise run nogame.lua
            string mainLuaPath = Path.Combine(gameDirectory, "main.lua");
            string noGamePath = "Lua/nogame.lua";

            string gamePath = File.Exists(mainLuaPath) ? mainLuaPath : noGamePath;

            // prepare to load file
            luaState["WORKING_DIR"] = File.Exists(mainLuaPath) ? gameDirectory : ".";
            luaState.NewTable("Steel");

            // load C# assembly
            luaState.LoadCLRPackage();
            luaState.DoString("import ('SteelEngine', 'SteelEngine.Lua')");

            // load file
            luaState.DoFile(gamePath);

            // Register all static methods in the Global class to the Lua state
            Type globalClassType = typeof(Global);
            foreach (var method in globalClassType.GetMethods(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public))
            {
                luaState.RegisterFunction(method.Name, method);
            }

            // lua event
            EngineProperties newProperties = (EngineProperties)luaState.GetFunction("Steel.Preload").Call(properties).First();

            // initialize window
            window = new Game(newProperties);
            window.onUpdateFrame += onUpdateFrame;
            window.onRenderFrame += onRenderFrame;
            window.onLoad += onLoad;

            // finalize
            using (window)
            {
                window.Run();
            }
        }

        private void onLoad()
        {
            // lua event
            luaState.GetFunction("Steel.Load").Call();

            Time.Initialize();
        }

        private double _deltaTime;
        private double _fpsTimer;
        private int _fpsCounter;

        private void onUpdateFrame(float deltaTime)
        {
            // lua event
            luaState.GetFunction("Steel.Update").Call(deltaTime);

            // system calls
            Input.Update();


            // Update FPS counter
            _deltaTime = deltaTime;
            _fpsTimer += deltaTime;
            _fpsCounter++;

            if (_fpsTimer >= 1.0) // If one second has passed
            {
                double fps = _fpsCounter / _fpsTimer;
                Console.WriteLine("FPS: " + fps);
                _fpsTimer = 0.0;
                _fpsCounter = 0;
            }
        }

        private void onRenderFrame()
        {
            // lua event
            luaState.GetFunction("Steel.Render").Call();
        }
    }
}
