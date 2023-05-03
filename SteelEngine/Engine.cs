using NLua;
using OpenTK.Graphics.ES11;

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
            luaState = new Lua();
            window = new Window(properties.width, properties.height, properties.title + $" v{properties.version}");
            window.onUpdateFrame += onUpdateFrame;
            window.onRenderFrame += onRenderFrame;
            window.onLoad += onLoad;

            using (window)
            {
                window.Run();
            }
        }

        private void AddLibariesToLua()
        {
            luaState.NewTable("Steel");
            luaState.LoadCLRPackage();
            luaState.DoString("import ('SteelEngine', 'SteelEngine')");
        }

        private void onLoad()
        {
            luaState["WORKING_DIR"] = ".";
            AddLibariesToLua();
            luaState.DoFile("main.lua");
            luaState.GetFunction("Steel.Load").Call();
        }

        private void onUpdateFrame(float deltaTime)
        {
            luaState.GetFunction("Steel.Update").Call(deltaTime);
            Input.Update();
        }

        private void onRenderFrame()
        {
            luaState.GetFunction("Steel.Render").Call();
        }
    }
}
