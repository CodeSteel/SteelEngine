using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SteelEngine
{
    public struct DrawRectangle
    {
        public Color color;
        public Vector2[] vertices;
        public int vertexArrayObject;
    }

    public class Draw
    {
        private static List<DrawRectangle> rectangles = new List<DrawRectangle>();
        private static int vao, vbo;
        private static Shader shader;

        public static void Initialize()
        {
            vao = GL.GenVertexArray();
            vbo = GL.GenBuffer();

            GL.BindVertexArray(vao);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, sizeof(float) * 6 * 4, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, sizeof(float) * 2, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);

            shader = new Shader("shaders/shader.vert", "shaders/shader.frag");
        }

        public static void DrawRectangle(int x, int y, int w, int h, Color color)
        {
            GL.BindVertexArray(vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);

            float[] vertices = new float[]
            {
                x, y,
                x + w, y,
                x + w, y + h,
                x, y + h
            };

            if (GL.IsBuffer(vbo))
            {
                GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, sizeof(float) * 6 * 2, vertices);

                GL.Color4(color.r, color.g, color.b, color.a);
                GL.DrawArrays(PrimitiveType.Quads, 0, 4);
            }
            else
                Console.WriteLine("VBO not created or deleted");

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
        }

        public static void Render()
        {
            foreach (DrawRectangle rect in rectangles)
            {
                shader.Use();

                GL.BindVertexArray(rect.vertexArrayObject);
                GL.BindBuffer(BufferTarget.ArrayBuffer, rect.vertexArrayObject);
                GL.BufferData(BufferTarget.ArrayBuffer, rect.vertices.Length * sizeof(float), rect.vertices, BufferUsageHint.DynamicDraw);

                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
                GL.EnableVertexAttribArray(0);
            }

            rectangles.Clear();
        }

    }
}
