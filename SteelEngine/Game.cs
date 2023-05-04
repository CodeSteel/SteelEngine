﻿using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using SteelEngine.Lua;

namespace SteelEngine
{
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
        #endregion

        EngineProperties engineProperties;

        public Game(EngineProperties properties) : base(GameWindowSettings.Default, new NativeWindowSettings() { Size = (properties.Width, properties.Height), Title = properties.Version == null ? properties.Title : $"{properties.Title} v{properties.Version}"})
        {
            engineProperties = properties;
            instance = this;
        }

        private static void SetWindowSize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            Renderer.OnWindowResize(width, height);
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
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Blend);

            GL.ClearColor(engineProperties.BackgroundColor.ToColor4());

            Renderer.Initilize(engineProperties.Width, engineProperties.Height);

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
        }

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
    }
}