using NLua;
using System;
using System.Diagnostics;
using System.Timers;

namespace SteelEngine.Lua
{
    public struct TimerObject
    {
        public System.Timers.Timer timer;
        public int timesRan;
        public LuaFunction callback;
    }

    /// <summary>
    /// This is used to bring all the time outputs into one class.
    /// </summary>
    public class Time
    {
        private static Stopwatch stopwatch = new Stopwatch();

        private static Dictionary<string, TimerObject> timers = new Dictionary<string, TimerObject>();

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

        /// <summary>
        /// Creates a new timer object.
        /// </summary>
        /// <param name="identifier">The name of the timer.</param>
        /// <param name="delay">How long before the timer goes off?</param>
        /// <param name="interval">How much times should the timer go off before being destroyed?</param>
        /// <param name="callback">The function to call when the timer has looped.</param>
        public static void CreateTimer(string identifier, float delay, float interval, LuaFunction callback)
        {
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = delay * 1000;
            timer.AutoReset = true;

            timers[identifier] = new TimerObject()
            {
                timer = timer,
                timesRan = 0,
                callback = callback
            };

            timer.Elapsed += (sender, e) =>
            {
                TimerObject timObj = timers[identifier];
                ++timObj.timesRan;

                timObj.callback.Call();

                if (interval > 0 && timObj.timesRan >= interval)
                {
                    RemoveTimer(identifier);
                }
            };

            timer.Start();
        }

        /// <summary>
        /// Returns if the timer exists.
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        public static bool TimerExists(string identifier)
        {
            bool exists = timers.TryGetValue(identifier, out TimerObject _);
            return exists;
        }

        /// <summary>
        /// Removes the given timer, if one exists.
        /// </summary>
        /// <param name="identifier"></param>
        public static void RemoveTimer(string identifier)
        {
            bool exists = timers.TryGetValue(identifier, out TimerObject timerObject);
            if (exists)
            {
                timerObject.timer.Stop();
                timerObject.timer.Dispose();
                timers.Remove(identifier);
            }
        }
    }
}
