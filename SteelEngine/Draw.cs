using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace SteelEngine
{
    public static class Draw
    {
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
    }
}
