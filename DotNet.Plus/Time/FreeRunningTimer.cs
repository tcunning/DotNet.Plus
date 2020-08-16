using System;
using System.Diagnostics;

namespace DotNet.Plus.Time
{
    public static class FreeRunningTimer
    {
        private static readonly Stopwatch _stopwatch = Stopwatch.StartNew();

        public static TimeSpan ElapsedTime => _stopwatch.Elapsed;
    }
}
