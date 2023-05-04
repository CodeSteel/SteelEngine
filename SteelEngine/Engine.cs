using NLua;
using NLua.Extensions;
using SteelEngine.Lua;

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
        private Game window;
        private NLua.Lua luaState;

        public Engine(EngineProperties properties, string gameDirectory)
        {
            // initialize lua
            luaState = new NLua.Lua();

            // check if main.lua exists, otherwise run nogame.lua
            string mainLuaPath = Path.Combine(gameDirectory, "main.lua");
            string noGamePath = "Lua/nogame.lua";

            string gamePath = File.Exists(mainLuaPath) ? mainLuaPath : noGamePath;

            // load file
            luaState["WORKING_DIR"] = File.Exists(mainLuaPath) ? gameDirectory : ".";
            luaState.NewTable("Steel");
            luaState.DoFile(gamePath);

            // load C# assembly
            luaState.LoadCLRPackage();
            luaState.DoString("import ('SteelEngine', 'SteelEngine.Lua')");

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

        private void onUpdateFrame(float deltaTime)
        {
            // lua event
            luaState.GetFunction("Steel.Update").Call(deltaTime);

            // system calls
            Input.Update();
        }

        private void onRenderFrame()
        {
            // lua event
            luaState.GetFunction("Steel.Render").Call();
        }
    }
}
