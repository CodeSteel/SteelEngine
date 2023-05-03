using Microsoft.VisualBasic.FileIO;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Runtime.CompilerServices;

namespace SteelEngine
{
    internal class Window : GameWindow
    {
        #region EVENTS
        public delegate void OnUpdateFrameHandler(float deltaTime);
        public event OnUpdateFrameHandler onUpdateFrame;

        public delegate void OnRenderFrameHandler();
        public event OnRenderFrameHandler onRenderFrame;

        public delegate void OnLoadHandler();
        public event OnLoadHandler onLoad;
        #endregion

        public Window(int width, int height, string title) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (width, height), Title = title })
        {
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            onUpdateFrame((float)args.Time);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            Input.Event_OnKeyDown(e.Key);
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit);

            Draw.DrawRectangle(50, 50, 100, 100, new Color(255, 0, 0));

            Draw.Render();

            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
                Console.WriteLine($"OpenGL Error: {error}");

            onRenderFrame();

            SwapBuffers();
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);

            // GL.Viewport(0, 0, this.Bounds.Size.X, this.Bounds.Size.Y);
            GL.ClearColor(Color4.Black);

            Draw.Initialize();

            onLoad();
        }

        protected override void OnUnload()
        {
            base.OnUnload();
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            base.OnKeyUp(e);

            Input.Event_OnKeyUp(e.Key);
        }
    }
}
