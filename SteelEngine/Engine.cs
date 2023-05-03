using NLua;

namespace SteelEngine
{
    public struct EngineProperties
    {
        public int width;
        public int height;
        public string title;
        public string version;
    }

    internal class Engine
    {
        private Window window;
        private Lua luaState;

        public Engine(EngineProperties properties)
        {
            // initialize window
            window = new Window(properties.width, properties.height, properties.title + $" v{properties.version}");
            window.onUpdateFrame += onUpdateFrame;
            window.onRenderFrame += onRenderFrame;
            window.onLoad += onLoad;

            // initialize other
            luaState = new Lua();

            // finalize
            using (window)
            {
                window.Run();
            }
        }

        private void onLoad()
        {
            luaState["WORKING_DIR"] = ".";

            // load C# assembly
            luaState.NewTable("Steel");
            luaState.LoadCLRPackage();
            luaState.DoString("import ('SteelEngine', 'SteelEngine')");

            // load file
            luaState.DoFile("main.lua");

            // lua event
            luaState.GetFunction("Steel.Load").Call();
        }

        private void onUpdateFrame(float deltaTime)
        {
            // system calls
            Input.Update();

            // lua event
            luaState.GetFunction("Steel.Update").Call(deltaTime);
        }

        private void onRenderFrame()
        {
            // lua event
            luaState.GetFunction("Steel.Render").Call();
        }
    }
}
