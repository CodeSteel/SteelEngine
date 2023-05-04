namespace SteelEngine.Lua
{
    /// <summary>
    /// The state of the Cursor when the application is focused.
    /// </summary>
    public enum CursorState
    {
        /// <summary>
        /// Cursor is visible and not locked to the window. Default setting.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Cursor is hidden when inside the window but is not locked to the window.
        /// </summary>
        HIDDEN = 1,

        /// <summary>
        /// Cursor is hidden and locked to the window.
        /// </summary>
        LOCKED = 2
    }
}
