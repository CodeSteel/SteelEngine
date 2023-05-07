using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SteelEngine
{
    /// <summary>
    /// Used to render shapes into the view.
    /// </summary>
    public static class Renderer
    {
        private static int VertexBufferObject;
        private static int VertexArrayObject;
        private static Shader ?ShaderProgram;

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

            // Initialize shader program
            ShaderProgram = new Shader("resources/shaders/shader.vert", "resources/shaders/shader.frag");

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

            float[] data = new float[vertices.Length + vertices.Length / 2 * 4];
            int dataIndex = 0;
            for (int i=0; i<vertices.Length; i+=2)
            {
                data[dataIndex++] = vertices[i];
                data[dataIndex++] = vertices[i + 1];
                data[dataIndex++] = r;
                data[dataIndex++] = g;
                data[dataIndex++] = b;
                data[dataIndex++] = a;
            }

            // Update Vertex Buffer Object with position data
            GL.BindBuffer(BufferTarget.ArrayBuffer, VertexBufferObject);
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.DynamicDraw);

            // Set vertex attribute pointers for position
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Set vertex attribute pointers for color
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 6 * sizeof(float), 2 * sizeof(float));
            GL.EnableVertexAttribArray(1);

            // Use shader
            ShaderProgram!.Use();

            // Draw the polygon using TriangleFan
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, vertices.Length / 2 - 1);
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
