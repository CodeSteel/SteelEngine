using OpenTK.Mathematics;

namespace SteelEngine
{
    public class Color
    {
        public int r;
        public int g;
        public int b;
        public int a;

        public Color(int _r, int _g, int _b)
        {
            r = _r;
            g = _g;
            b = _b;
            a = 255;
        }

        public Color(int _r, int _g, int _b, int _a)
        {
            r = _r;
            g = _g;
            b = _b;
            a = _a;
        }

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
