using System;
using System.Diagnostics;

namespace SteelEngine.Lua
{
    /// <summary>
    /// This is used to bring all the time outputs into one class.
    /// </summary>
    public class Time
    {
        private static Stopwatch stopwatch = new Stopwatch();

        /// <summary>
        /// Used internally to start the time process.
        /// (Warning) There is no need to call this yourself.
        /// </summary>
        public static void Initialize()
        {
            stopwatch.Start();
        }

        /// <summary>
        /// Returns the amount of time the appliction has been running.
        /// </summary>
        /// <returns></returns>
        public static double GetTime()
        {
            if (!stopwatch.IsRunning)
            {
                stopwatch.Start();
            }

            return stopwatch.ElapsedMilliseconds / 1000f;
        }

        /// <summary>
        /// Returns the amount of time it took to render the last frame in seconds.
        /// </summary>
        /// <returns></returns>
        public static float GetDeltaTime()
        {
            return (float)Game.instance.RenderTime;
        }
    }
}
