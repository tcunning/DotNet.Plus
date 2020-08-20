using System;
using System.Diagnostics;

namespace DotNet.Plus.Time
{
    /// <summary>
    /// Upon first access sets up a free running counter that runs for the duration of the
    /// application's life cycle.
    /// </summary>
    public static class FreeRunningTimer
    {
        private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        /// <summary>
        /// Returns true if the underlying implementation is using a high resolution timer
        /// </summary>
        public static bool IsHighResolution => Stopwatch.IsHighResolution;

        /// <summary>
        /// Amount of time that has passed since the FreeRunningTimer was first accessed
        /// </summary>
        public static TimeSpan ElapsedTime => _stopwatch.Elapsed;
    }
}
