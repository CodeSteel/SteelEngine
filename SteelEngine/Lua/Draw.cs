using System.Drawing;
using System.Xml.Serialization;

namespace SteelEngine.Lua
{
    /// <summary>
    /// Holds information for loaded fonts.
    /// </summary>
    public struct FontObject
    {
        /// <summary>
        /// The path to the font.
        /// </summary>
        public string fontPath;

        /// <summary>
        /// The size of the font.
        /// </summary>
        public int size;

        /// <summary>
        /// The generated font.
        /// </summary>
        public Font font;
    }

    /// <summary>
    /// Used for sending graphics to the renderer.
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform("windows")]
    public static class Draw
    {
        private static Dictionary<string, FontObject> FontObjects = new Dictionary<string, FontObject>();
        private static Dictionary<string, Texture> TextureObjects = new Dictionary<string, Texture>();

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
        /// Draws a textured rectangle at the position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="textureName"></param>
        /// <param name="color"></param>
        public static void DrawTexturedRectangle(float x, float y, float width, float height, string textureName, Color color)
        {
            Texture texture = TextureObjects[textureName];
            DrawTexturedRectangle(x, y, width, height, texture, color);
        }

        /// <summary>
        /// Draws a textured rectangle at the position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="texture"></param>
        /// <param name="color"></param>
        public static void DrawTexturedRectangle(float x, float y, float width, float height, Texture texture, Color color)
        {
            float[] vertices = new float[]
{
                x, y,
                x + width, y,
                x + width, y + height,
                x, y + height
            };

            float[] texCoords = new float[]
            {
                0.0f, 0.0f,
                1.0f, 0.0f,
                1.0f, 1.0f,
                0.0f, 1.0f
            };

            Renderer.DrawTexturedPoly(vertices, texCoords, texture, color);
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
                    float x = center.x + roundness * MathF.Cos(angle);
                    float y = center.y + roundness * MathF.Sin(angle);
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

            for (int i = 0; i <= segments; i++)
            {
                float angle = (float)i / segments * 2 * MathF.PI;
                float _x = x + radius * MathF.Cos(angle);
                float _y = y + radius * MathF.Sin(angle);

                vertices.Add(_x);
                vertices.Add(_y);
            }

            Renderer.DrawPoly(vertices.ToArray(), color);
        }

        /// <summary>
        /// Registers a font to be used in runtime.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="fontPath"></param>
        /// <param name="fontSize"></param>
        public static void CreateFont(string identifier, string fontPath, int fontSize)
        {
            // Initialize the font object
            FontObject obj = new FontObject()
            {
                fontPath = fontPath,
                size = fontSize
            };

            // Generate the font
            Font font = new Font(fontPath, fontSize);
            obj.font = font;

            // Add the font object
            FontObjects.Add(identifier, obj);
        }

        /// <summary>
        /// Registers a texture.
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="path"></param>
        public static void CreateTexture(string identifier, string path)
        {
            Texture texture = Texture.LoadFromFile(path);
            TextureObjects.Add(identifier, texture);
        }

        /// <summary>
        /// Draws text to the screen.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontName"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public static void DrawText(string text, string fontName, int x, int y, Color color)
        {
            Font font = FontObjects[fontName].font;
            Renderer.DrawText(text, font, new Vector2(x, y), color);
        } 
    }
}
