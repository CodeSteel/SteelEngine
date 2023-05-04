namespace SteelEngine.Lua
{
    /// <summary>
    /// Represents Lua's global class.
    /// </summary>
    public class Global
    {
        /// <summary>
        /// Returns a lerped value from a to b using t as Time.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static float Lerp(float a, float b, float t)
        {
            return a + (b - a) * Math.Clamp(t, 0f, 1f);
        }

        /// <summary>
        /// The width of the viewport screen.
        /// </summary>
        /// <returns></returns>
        public static float ScrW()
        {
            (int width, _) = Window.GetSize();
            return width;
        }

        /// <summary>
        /// The height of the viewport screen.
        /// </summary>
        /// <returns></returns>
        public static float ScrH()
        {
            (_, int height) = Window.GetSize();
            return height;
        }

        /// <summary>
        /// Clamps a value between a min and max.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float Clamp(float value, float min, float max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
    }
}
