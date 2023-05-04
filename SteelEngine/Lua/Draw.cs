using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SteelEngine.Lua
{
    /// <summary>
    /// This class is used for sending graphics to the renderer.
    /// </summary>
    public static class Draw
    {
        /// <summary>
        /// Draws a rectangle at the position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        public static void DrawRectangle(float x, float y, float width, float height, Color color)
        {
            float[] vertices = new float[]
            {
                x, y,
                x + width, y,
                x + width, y + height,
                x, y + height
            };

            Renderer.DrawPoly(vertices, color);
        }

        /// <summary>
        /// Draws a rectangle at the position, taking in the roundness.
        /// </summary>
        /// <param name="roundness">This is a 0.0f-1.0f value, measures the amount of roundness the corners have.</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <param name="segmentsPerCorner"></param>
        public static void DrawRoundedRectangle(float roundness, float x, float y, float width, float height, Color color, int segmentsPerCorner = 8)
        {
            List<float> vertices = new List<float>();

            // Helper function to add vertices for each corner
            void AddCornerVertices(Vector2 center, float startAngle)
            {
                for (int i = 0; i <= segmentsPerCorner; i++)
                {
                    float angle = startAngle + (float)i / segmentsPerCorner * MathF.PI / 2;
                    float x = center.X + roundness * MathF.Cos(angle);
                    float y = center.Y + roundness * MathF.Sin(angle);
                    vertices.Add(x);
                    vertices.Add(y);
                }
            }

            // Add vertices for each corner
            AddCornerVertices(new Vector2(x + roundness, y + roundness), MathF.PI);
            AddCornerVertices(new Vector2(x + width - roundness, y + roundness), -MathF.PI / 2);
            AddCornerVertices(new Vector2(x + width - roundness, y + height - roundness), 0);
            AddCornerVertices(new Vector2(x + roundness, y + height - roundness), MathF.PI / 2);

            // Draw the rounded rectangle
            Renderer.DrawPoly(vertices.ToArray(), color);
        }

        /// <summary>
        /// Draws a circle at the position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        /// <param name="color"></param>
        /// <param name="segments"></param>
        public static void DrawCircle(float x, float y, float radius, Color color, int segments = 16)
        {
            List<float> vertices = new List<float>();

            for (int i=0; i<=segments; i++)
            {
                float angle = (float)i / segments * 2 * MathF.PI;
                float _x = x + radius * MathF.Cos(angle);
                float _y = y + radius * MathF.Sin(angle);

                vertices.Add(_x);
                vertices.Add(_y);
            }

            Renderer.DrawPoly(vertices.ToArray(), color);
        }

    }
}
