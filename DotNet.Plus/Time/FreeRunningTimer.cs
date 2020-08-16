using System;
using System.Diagnostics;

namespace DotNet.Plus.Time
{
    public static class FreeRunningTimer
    {
        private static readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        public static TimeSpan ElapsedTime => Stopwatch.Elapsed;
    }
}
