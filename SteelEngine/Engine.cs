using NLua;

namespace SteelEngine
{
    public struct EngineProperties
    {
        public int Width;
        public int Height;
        public string Title;
        public string? Version;
        public Color BackgroundColor;
    }

    internal class Engine
    {
        private Window window;
        private Lua luaState;

        public Engine(EngineProperties properties)
        {
            // initialize lua
            luaState = new Lua();

            // load file
            luaState["WORKING_DIR"] = ".";
            luaState.NewTable("Steel");
            luaState.DoFile("main.lua");

            // load C# assembly
            luaState.LoadCLRPackage();
            luaState.DoString("import ('SteelEngine', 'SteelEngine')");

            // lua event
            EngineProperties newProperties = (EngineProperties)luaState.GetFunction("Steel.Preload").Call(properties).First();

            // initialize window
            window = new Window(newProperties);
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
