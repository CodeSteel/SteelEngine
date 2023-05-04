using OpenTK.Mathematics;

namespace SteelEngine.Lua
{
    /// <summary>
    /// A typical Color class used to store `rgba` values.
    /// </summary>
    public class Color
    {
        public int r;
        public int g;
        public int b;
        public int a;

        /// <summary>
        /// Creates a new Color object with the rgb value format.
        /// The Alpha will default to 255.
        /// </summary>
        /// <param name="_r"></param>
        /// <param name="_g"></param>
        /// <param name="_b"></param>
        public Color(int _r, int _g, int _b)
        {
            r = _r;
            g = _g;
            b = _b;
            a = 255;
        }

        /// <summary>
        /// Creates a new Color object with the `rgba` value format.
        /// </summary>
        /// <param name="_r"></param>
        /// <param name="_g"></param>
        /// <param name="_b"></param>
        /// <param name="_a"></param>
        public Color(int _r, int _g, int _b, int _a)
        {
            r = _r;
            g = _g;
            b = _b;
            a = _a;
        }

        /// <summary>
        /// Copies itself into a new color and takes the alpha value
        /// </summary>
        /// <param name="_a"></param>
        /// <returns></returns>
        public Color WithAlpha(int _a)
        {
            return new Color(r, g, b, _a);
        }

        public (int, int, int, int) Value()
        {
            return (r, g, b, a);
        }

        public Color4 ToColor4()
        {
            return new Color4(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
        }
    }
}
