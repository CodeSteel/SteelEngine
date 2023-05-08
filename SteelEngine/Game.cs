using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SteelEngine.Lua;

namespace SteelEngine
{
    /// <summary>
    /// The Game Window of the application.
    /// Everything is called from here.
    /// </summary>
    internal class Game : GameWindow
    {
        public static Game instance;

        #region EVENTS
        public delegate void OnUpdateFrameHandler(float deltaTime);
        public event OnUpdateFrameHandler ?onUpdateFrame;

        public delegate void OnRenderFrameHandler();
        public event OnRenderFrameHandler ?onRenderFrame;

        public delegate void OnLoadHandler();
        public event OnLoadHandler ?onLoad;

        public delegate void OnWindowSizeChanged(int w, int h);
        public event OnWindowSizeChanged? onWindowSizeChanged;
        #endregion

        public EngineProperties engineProperties;

        public Game(EngineProperties properties) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (properties.Width, properties.Height), Title = properties.Version == null ? properties.Title : $"{properties.Title} v{properties.Version}"})
        {
            instance = this;
            engineProperties = properties;
        }

        private static void SetWindowSize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            // opengl
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // event
            if (onRenderFrame != null)
                onRenderFrame();

            // check for errors
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                Console.WriteLine($"OpenGL Error: {error}");

            // finalize
            SwapBuffers();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            // initiailize gl
            //GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

            GL.ClearColor(engineProperties.BackgroundColor.ToColor4());

            Renderer.Initialize(engineProperties.Width, engineProperties.Height);

            // event
            if (onLoad != null) 
                onLoad();
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            // event
            if (onUpdateFrame != null)
                onUpdateFrame((float)args.Time);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            SetWindowSize(e.Width, e.Height);

            if (onWindowSizeChanged != null)
                onWindowSizeChanged(e.Width, e.Height);
        }

        #region Input  Events
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);
            Input.Event_OnKeyDown(e.Key);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);
            Input.Event_OnKeyUp(e.Key);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            base.OnMouseMove(e);
            Input.Event_OnMouseMove(new Lua.Vector2(e.X, e.Y), new Lua.Vector2(e.DeltaX, e.DeltaY));
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            Input.Event_OnMouseDown(e.Button);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
            Input.Event_OnMouseUp(e.Button);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);
            Input.Event_OnMouseWheel(e.OffsetX, e.OffsetY);
        }

        #endregion
    }
}
