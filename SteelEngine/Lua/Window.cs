namespace SteelEngine.Lua
{
    /// <summary>
    /// This is used to call functions to the Game Window.
    /// </summary>
    public class Window
    {
        /// <summary>
        /// Sets the window's cursor state.
        /// </summary>
        /// <param name="state"></param>
        public static void SetCursorState(CursorState state)
        {
            Game.instance.CursorState = (OpenTK.Windowing.Common.CursorState)state;
        }

        /// <summary>
        /// Returns the width and height of the window.
        /// </summary>
        /// <returns></returns>
        public static (int, int) GetSize()
        {
            return (Game.instance.Size.X, Game.instance.Size.Y);
        }

        /// <summary>
        /// Returns true if the game is running in Fullscreen mode.
        /// </summary>
        /// <returns></returns>
        public static bool GetFullscreen()
        {
            return Game.instance.IsFullscreen;
        }

        /// <summary>
        /// Returns true if the game is the focused window.
        /// </summary>
        /// <returns></returns>
        public static bool GetFocused()
        {
            return Game.instance.IsFocused;
        }

        /// <summary>
        /// Returns the current Cursor State.
        /// </summary>
        /// <returns></returns>
        public static CursorState GetCursorState()
        {
            return (CursorState)Game.instance.CursorState;
        }

        /// <summary>
        /// Terminates the application.
        /// </summary>
        public static void Close()
        {
            Game.instance.Close();
        }
    }
}
