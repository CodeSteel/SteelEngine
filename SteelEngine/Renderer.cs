using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SteelEngine
{
    public static class Renderer
    {
        private static int VertexBufferObject;
        private static int VertexArrayObject;
        private static Shader ?ShaderProgram;

        private static int _uProjectionLocation;
        private static int _uModelViewLocation;

        public static void Initilize(int screenWidth, int screenHeight)
        {
            // Create and bind Vertex Array Object
            VertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(VertexArrayObject);

            // Create and bind Vertex Buffer Object
            VertexBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);

            // Initialize shader program
            ShaderProgram = new Shader("resources/shaders/shader.vert", "resources/shaders/shader.frag");
            ShaderProgram.Use();

            _uProjectionLocation = GL.GetUniformLocation(ShaderProgram.Handle, "uProjection");
            _uModelViewLocation = GL.GetUniformLocation(ShaderProgram.Handle, "uModelView");


            // Initialize the projection matrix
            SetupView(screenWidth, screenHeight);
        }


        /// <summary>
        /// Draws a polygon to the screen
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="color"></param>
        public static void DrawPoly(float[] vertices, Lua.Color color)
        {
            // Normalize color components
            float r = color.r / 255.0f;
            float g = color.g / 255.0f;
            float b = color.b / 255.0f;
            float a = color.a / 255.0f;

            // Update Vertex Buffer Object with position data
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.DynamicDraw);

            // Set vertex attribute pointers for position
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Create and bind a new buffer for color data
            int colorBufferObject = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, colorBufferObject);

            // Prepare and upload color data to the GPU
            float[] colors = new float[vertices.Length / 2 * 4];
            for (int i = 0; i < colors.Length; i += 4)
            {
                colors[i] = r;
                colors[i + 1] = g;
                colors[i + 2] = b;
                colors[i + 3] = a;
            }
            GL.BufferData(BufferTarget.ArrayBuffer, colors.Length * sizeof(float), colors, BufferUsageHint.DynamicDraw);

            // Set vertex attribute pointers for color
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(1);

            // Use shader
            ShaderProgram!.Use();

            // Draw the polygon using TriangleFan
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertices.Length / 2);

            // Clean up the color buffer object
            GL.DeleteBuffer(colorBufferObject);
        }

        public static void OnWindowResize(int screenWidth, int screenHeight)
        {
            SetupView(screenWidth, screenHeight);
        }

        private static void SetupView(int screenWidth, int screenHeight)
        {
            Matrix4 projection = Matrix4.CreateOrthographicOffCenter(0, screenWidth, screenHeight, 0, -1, 1);

            // Set the projection uniform in the shader
            ShaderProgram!.Use();
            GL.UniformMatrix4(_uProjectionLocation, false, ref projection);

            Matrix4 flipX = new Matrix4(
                -1, 0, 0, screenWidth,
                 0, 1, 0, 0,
                 0, 0, 1, 0,
                 0, 0, 0, 1
            );

            // Set the model-view uniform in the shader
            ShaderProgram!.Use();
            GL.UniformMatrix4(_uModelViewLocation, false, ref flipX);
        }
    }
}
