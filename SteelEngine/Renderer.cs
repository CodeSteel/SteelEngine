using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using SteelEngine.Lua;
using System.Drawing;

namespace SteelEngine
{
    /// <summary>
    /// Used to render shapes into the view.
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public static class Renderer
    {
        private static int VertexBufferObject;
        private static int VertexArrayObject;

        private static Shader? ShaderProgram;

        private static int _uProjectionLocation;
        private static int _uModelViewLocation;

        /// <summary>
        /// Called internally to initialize the renderer.
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        public static void Initialize(int screenWidth, int screenHeight)
        {
            // Create and bind Vertex Array Object
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // Create and bind Vertex Buffer Object
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // Initialize shader programs
            ShaderProgram = new Shader("resources/shaders/shader.vert", "resources/shaders/shader.frag");

            // Initialize the projection matrix
            SetupView(screenWidth, screenHeight);
        }


        /// <summary>
        /// Renders a polygon to the screen
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="color"></param>
        public static void DrawPoly(float[] vertices, Lua.Color? color)
        {
            color = color == null ? Lua.Color.White : color;

            // Normalize color components
            float r = color.r / 255.0f;
            float g = color.g / 255.0f;
            float b = color.b / 255.0f;
            float a = color.a / 255.0f;

            float[] data = new float[vertices.Length + vertices.Length / 2 * 6];
            int dataIndex = 0;
            for (int i = 0; i < vertices.Length; i += 2)
            {
                data[dataIndex++] = vertices[i];
                data[dataIndex++] = vertices[i + 1];
                data[dataIndex++] = r;
                data[dataIndex++] = g;
                data[dataIndex++] = b;
                data[dataIndex++] = a;
                data[dataIndex++] = 1; // give it a texture coord of 1,1 so when it's multiplied, it won't interupt.
                data[dataIndex++] = 1;
            }

            // Update Vertex Buffer Object with position data
            // GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.DynamicDraw);

            // Set vertex attribute pointers for position
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Set vertex attribute pointers for color
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Set vertex attribute pointers for tex coord
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            // Use shader
            ShaderProgram!.Use();

            // Draw the polygon using TriangleFan
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertices.Length / 2);
        }

        /// <summary>
        /// Renders a textured polygon to the screen.
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="texCoords"></param>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        public static void DrawTexturedPoly(float[] vertices, float[] texCoords, Texture texture, Lua.Color? color)
        {
            color = color == null ? Lua.Color.White : color;

            // Normalize color components
            float r = color.r / 255.0f;
            float g = color.g / 255.0f;
            float b = color.b / 255.0f;
            float a = color.a / 255.0f;

            float[] data = new float[vertices.Length + vertices.Length / 2 * 6];
            int dataIndex = 0;
            for (int i = 0; i < vertices.Length; i += 2)
            {
                data[dataIndex++] = vertices[i];
                data[dataIndex++] = vertices[i + 1];
                data[dataIndex++] = r;
                data[dataIndex++] = g;
                data[dataIndex++] = b;
                data[dataIndex++] = a;
                data[dataIndex++] = texCoords[i];
                data[dataIndex++] = texCoords[i + 1];
            }

            GL.BindVertexArray(VertexArrayObject);

            // Update our buffer with the object data
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.DynamicDraw);

            // Set vertex attribute pointers for position
            // position
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // color
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 8 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // texture coord
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));
            GL.EnableVertexAttribArray(2);

            // Use texture
            texture.Use(OpenTK.Graphics.OpenGL4.TextureUnit.Texture0);

            // Use shader
            ShaderProgram!.Use();

            // Draw the polygon using TriangleFan
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertices.Length / 2);
        }

        /// <summary>
        /// Renders a text to the screen.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="font"></param>
        /// <param name="position"></param>
        /// <param name="color"></param>
        public static void DrawText(string text, Font font, Lua.Vector2 position, Lua.Color? color)
        {
            color = color == null ? Lua.Color.White : color;

            Bitmap bitmap = new Bitmap(Game.instance.Size.X, Game.instance.Size.Y);

            Graphics graphics = Graphics.FromImage(bitmap);
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;

            graphics.Clear(System.Drawing.Color.Transparent);

            graphics.DrawString(text, font, Brushes.White, new Point(0, 0));

            Texture texture = Texture.FromBitmap(bitmap);

            graphics.Dispose();
            bitmap.Dispose();

            // Calculate the texture coordinates for a rectangle that covers the entire screen
            Draw.DrawTexturedRectangle(position.x, position.y, texture.Width, texture.Height, texture, color);
        }

        /// <summary>
        /// This event is called internally when the window is resized.
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        public static void Event_OnWindowResize(int screenWidth, int screenHeight)
        {
            SetupView(screenWidth, screenHeight);
        }

        /// <summary>
        /// Sets up the view matrix projection. (origin is top-left.)
        /// </summary>
        /// <param name="screenWidth"></param>
        /// <param name="screenHeight"></param>
        private static void SetupView(int screenWidth, int screenHeight)
        {
            // Set the projection uniform in the shader
            ShaderProgram!.Use();

            // get the projection / view location
            _uProjectionLocation = GL.GetUniformLocation(ShaderProgram.Handle, "uProjection");
            _uModelViewLocation = GL.GetUniformLocation(ShaderProgram.Handle, "uModelView");

            // create the projection matrix
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, screenWidth, screenHeight, 0, -1, 1);
            Matrix4 flipX = new Matrix4(
                -1, 0, 0, screenWidth,
                 0, 1, 0, 0,
                 0, 0, 1, 0,
                 0, 0, 0, 1
            );

            // Set the model-view uniform in the shader
            GL.UniformMatrix4(_uProjectionLocation, false, ref projection);
            GL.UniformMatrix4(_uModelViewLocation, false, ref flipX);
        }
    }
}
