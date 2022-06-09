using DotNet.Plus.Math;
using System;

namespace TestConsole
{
    class Program
    {
        public static readonly PerformanceTimerOption TimerOptions = PerformanceTimerOption.AutoStartOnCreate | PerformanceTimerOption.Verbose;
        public static readonly TimeSpan WarningTimeSpan = TimeSpan.FromMilliseconds(5000);

        static void Main(string[] args)
        {
            using (new PerformanceTimer("", $"Template Version", WarningTimeSpan, TimerOptions))
            {
                for (int index = 0; index < 4_000_000; index += 1)
                    Clamp.Value((int)50, (int)10, (int)100);
            }

            Console.WriteLine();

            using (new PerformanceTimer("", $"Int Version", WarningTimeSpan, TimerOptions))
            {
                for (int index = 0; index < 4_000_000; index += 1)
                    Clamp.ValueInt(50, 10, 100);
            }

            Console.WriteLine();

            using (new PerformanceTimer("", $"IL Version", WarningTimeSpan, TimerOptions))
            {
                for (int index = 0; index < 4_000_000; index += 1)
                    DotNet.Plus.Fast.Clamp.Value(50, 10, 100);
            }

            Console.WriteLine();

            using (new PerformanceTimer("", $"IL Template Version", WarningTimeSpan, TimerOptions))
            {
                for (int index = 0; index < 4_000_000; index += 1)
                    DotNet.Plus.Fast.Clamp.Value<int>(50, 10, 100);
            }

        }
    }
}
