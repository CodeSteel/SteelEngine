using OpenTK.Windowing.GraphicsLibraryFramework;

namespace SteelEngine.Lua
{
    internal class Input
    {
        private static List<Keys> currentState = new List<Keys>();
        private static List<Keys> previousState = new List<Keys>();

        /// <summary>
        /// Called every frame to update the input state.
        /// </summary>
        public static void Update()
        {
            previousState.Clear();
            for (int i = 0; i < currentState.Count; i++)
            {
                previousState.Add(currentState[i]);
            }
        }

        /// <summary>
        /// This method is called when a key is pressed.
        /// </summary>
        /// <param name="key"></param>
        public static void Event_OnKeyDown(Keys key)
        {
            currentState.Add(key);
        }

        /// <summary>
        /// This method is called when a key is released.
        /// </summary>
        /// <param name="key"></param>
        public static void Event_OnKeyUp(Keys key)
        {
            currentState.Remove(key);
        }

        /// <summary>
        /// Returns if the key is being pressed.
        /// </summary>
        /// <param name="key">Use a KeyCode.</param>
        /// <returns></returns>
        public static bool GetKey(int key)
        {
            if (currentState.Contains((Keys)key))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Retuns if the key was pressed.
        /// </summary>
        /// <param name="key">Use a KeyCode</param>
        /// <returns></returns>
        public static bool GetKeyDown(int key)
        {
            if (currentState.Contains((Keys)key) && !previousState.Contains((Keys)key))
            {
                return true;
            }

            return false;
        }
    }

    public static class KeyCode
    {
        public const int Unknown = -1;
        public const int Space = (int)Keys.Space;
        public const int Apostrophe = (int)Keys.Apostrophe;
        public const int Comma = (int)Keys.Comma;
        public const int Minus = (int)Keys.Minus;
        public const int Period = (int)Keys.Period;
        public const int Slash = (int)Keys.Slash;
        public const int Num0 = (int)Keys.D0;
        public const int Num1 = (int)Keys.D1;
        public const int Num2 = (int)Keys.D2;
        public const int Num3 = (int)Keys.D3;
        public const int Num4 = (int)Keys.D4;
        public const int Num5 = (int)Keys.D5;
        public const int Num6 = (int)Keys.D6;
        public const int Num7 = (int)Keys.D7;
        public const int Num8 = (int)Keys.D8;
        public const int Num9 = (int)Keys.D9;
        public const int Semicolon = (int)Keys.Semicolon;
        public const int Equal = (int)Keys.Equal;
        public const int A = (int)Keys.A;
        public const int B = (int)Keys.B;
        public const int C = (int)Keys.C;
        public const int D = (int)Keys.D;
        public const int E = (int)Keys.E;
        public const int F = (int)Keys.F;
        public const int G = (int)Keys.G;
        public const int H = (int)Keys.H;
        public const int I = (int)Keys.I;
        public const int J = (int)Keys.J;
        public const int K = (int)Keys.K;
        public const int L = (int)Keys.L;
        public const int M = (int)Keys.M;
        public const int N = (int)Keys.N;
        public const int O = (int)Keys.O;
        public const int P = (int)Keys.P;
        public const int Q = (int)Keys.Q;
        public const int R = (int)Keys.R;
        public const int S = (int)Keys.S;
        public const int T = (int)Keys.T;
        public const int U = (int)Keys.U;
        public const int V = (int)Keys.V;
        public const int W = (int)Keys.W;
        public const int X = (int)Keys.X;
        public const int Y = (int)Keys.Y;
        public const int Z = (int)Keys.Z;
        public const int LeftBracket = (int)Keys.LeftBracket;
        public const int Backslash = (int)Keys.Backslash;
        public const int RightBracket = (int)Keys.RightBracket;
        public const int GraveAccent = (int)Keys.GraveAccent;
        public const int World1 = -1; /* non-US #1 */
        public const int World2 = -1; /* non-US #2 */
        public const int Escape = (int)Keys.Escape;
        public const int Enter = (int)Keys.Enter;
        public const int Tab = (int)Keys.Tab;
        public const int Backspace = (int)Keys.Backslash;
        public const int Insert = (int)Keys.Insert;
        public const int Delete = (int)Keys.Delete;
        public const int Right = (int)Keys.Right;
        public const int Left = (int)Keys.Left;
        public const int Down = (int)Keys.Down;
        public const int Up = (int)Keys.Up;
        public const int PageUp = (int)Keys.PageUp;
        public const int PageDown = (int)Keys.PageDown;
        public const int Home = (int)Keys.Home;
        public const int End = (int)Keys.End;
        public const int CapsLock = (int)Keys.CapsLock;
        public const int ScrollLock = (int)Keys.ScrollLock;
        public const int NumLock = (int)Keys.NumLock;
        public const int PrintScreen = -1;
        public const int Pause = (int)Keys.Pause;
        public const int F1 = (int)Keys.F1;
        public const int F2 = (int)Keys.F2;
        public const int F3 = (int)Keys.F3;
        public const int F4 = (int)Keys.F4;
        public const int F5 = (int)Keys.F5;
        public const int F6 = (int)Keys.F6;
        public const int F7 = (int)Keys.F7;
        public const int F8 = (int)Keys.F8;
        public const int F9 = (int)Keys.F9;
        public const int F10 = (int)Keys.F10;
        public const int F11 = (int)Keys.F11;
        public const int F12 = (int)Keys.F12;
        public const int F13 = (int)Keys.F13;
        public const int F14 = (int)Keys.F14;
        public const int F15 = (int)Keys.F15;
        public const int F16 = -1;
        public const int F17 = -1;
        public const int F18 = -1;
        public const int F19 = -1;
        public const int F20 = -1;
        public const int F21 = -1;
        public const int F22 = -1;
        public const int F23 = -1;
        public const int F24 = -1;
        public const int Keypad0 = (int)Keys.KeyPad0;
        public const int Keypad1 = (int)Keys.KeyPad1;
        public const int Keypad2 = (int)Keys.KeyPad2;
        public const int Keypad3 = (int)Keys.KeyPad3;
        public const int Keypad4 = (int)Keys.KeyPad4;
        public const int Keypad5 = (int)Keys.KeyPad5;
        public const int Keypad6 = (int)Keys.KeyPad6;
        public const int Keypad7 = (int)Keys.KeyPad7;
        public const int Keypad8 = (int)Keys.KeyPad8;
        public const int Keypad9 = (int)Keys.KeyPad9;
        public const int KeypadDecimal = (int)Keys.KeyPadDecimal;
        public const int KeypadDivide = (int)Keys.KeyPadDivide;
        public const int KeypadMultiply = (int)Keys.KeyPadMultiply;
        public const int KeypadSubtract = (int)Keys.KeyPadSubtract;
        public const int KeypadAdd = (int)Keys.KeyPadAdd;
        public const int KeypadEnter = -1;
        public const int LeftShift = (int)Keys.LeftShift;
        public const int LeftControl = (int)Keys.LeftControl;
        public const int LeftAlt = (int)Keys.LeftAlt;
        public const int LeftWindows = -1;
        public const int RightShift = (int)Keys.RightShift;
        public const int RightControl = (int)Keys.RightControl;
        public const int RightAlt = (int)Keys.RightAlt;
        public const int RightWindows = -1;
    }
}
