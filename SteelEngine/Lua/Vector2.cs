namespace SteelEngine.Lua
{
    /// <summary>
    /// Represents a generic 2D vector array.
    /// </summary>
    public class Vector2
    {
        /// <summary>
        /// The X position.
        /// </summary>
        public float x;

        /// <summary>
        /// The Y position.
        /// </summary>
        public float y;

        /// <summary>
        /// Create a Vector2 object with arguments.
        /// </summary>
        /// <param name="_x"></param>
        /// <param name="_y"></param>
        public Vector2(float _x, float _y)
        {
            x = _x;
            y = _y;
        }

        /// <summary>
        /// Create a Vector2 object, starting at 0,0.
        /// </summary>
        public Vector2()
        {
            x = 0;
            y = 0;
        }
    }
}
