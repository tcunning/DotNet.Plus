using DotNet.Plus.Pattern;
using System;
using System.Diagnostics;
using System.Threading;

namespace TestConsole
{
    [Flags]
    public enum PerformanceTimerOption
    {
        OnShowStart = 0x01,
        OnShowStopTotalTimeInMs = 0x02,
        AutoStartOnCreate = 0x04,

        None = 0x00,
        Verbose = OnShowStart | OnShowStopTotalTimeInMs,
    }


    /// <summary>
    /// Dispose is not required, but it allows for the using pattern.  However, when constructing a new PerformanceTimer be sure to specify auto start.  
    /// </summary>
    public struct PerformanceTimer : IDisposable
    {
        private static ObjectPool<Stopwatch> _stopwatchObjectPool = ObjectPool<Stopwatch>.MakeObjectPool<Stopwatch>();

        public PerformanceTimerOption Option { get; }

        public string LogTag { get; }

        public Func<string> MakeWatchDetailDescription; // Used to generation description to show

        /// <summary>
        /// This is true iff the PerformanceWatch has been configured with the non-default constructor.  If this is true, then this is just a "default" instance of
        /// PerformanceWatch with nothing configured but default values.
        /// </summary>
        public bool IsConfigured { get; }

        public bool IsRunning => _stopwatch?.IsRunning ?? false;

        private Stopwatch? _stopwatch;

        private TimeSpan? StopTimeWarning { get; }   // We will warning and or break when the timer is stopped if the stop value is greater then this given value.

        #region Constructors
        public PerformanceTimer(string logTag, string message, TimeSpan? stopTimeWarning = null, PerformanceTimerOption option = PerformanceTimerOption.Verbose | PerformanceTimerOption.AutoStartOnCreate) : this(logTag, (object)message, stopTimeWarning, option)
        {
        }

        /// <summary>
        /// Create a PerformanceWatch
        /// </summary>
        /// <param name="logTag">Used as the LogTag when logging messages</param>
        /// <param name="detailObject">Use to give details regarding what is being watch, this can be just a string description, and object (in which case we will show the object's type name), or null.
        /// a lambda can be assigned to MakeWatchDetailDescription which allows for total customization</param>
        /// <param name="option">See PerformanceWatchOption</param>
        /// <param name="stopTimeWarning">If not null, when stop is called a warning will be output if the time is greater then this value</param>
        public PerformanceTimer(string logTag, object detailObject, TimeSpan? stopTimeWarning = null, PerformanceTimerOption option = PerformanceTimerOption.Verbose | PerformanceTimerOption.AutoStartOnCreate)
        {
            Option = option;
            LogTag = logTag;

            if (detailObject is string)
                MakeWatchDetailDescription = () => detailObject as string ?? $"Null Reference Error {nameof(detailObject)}";
            else if (detailObject != null)
                MakeWatchDetailDescription = () => detailObject.GetType().Name;
            else
                MakeWatchDetailDescription = () => String.Empty;

            _stopwatch = null;

            StopTimeWarning = stopTimeWarning;

            IsConfigured = true;

            if (option.HasFlag(PerformanceTimerOption.AutoStartOnCreate))
                Start();
        }
        #endregion

        public void Start()
        {
            if (IsRunning)
            {
                Console.WriteLine( $"Ignoring Start as timer already started {MakeWatchDetailDescription()}");
                return;
            }

            _stopwatch = _stopwatchObjectPool.TakeObject();
            _stopwatch.Restart();   // Make sure we reset the timer.

            if (Option.HasFlag(PerformanceTimerOption.OnShowStart))
                Console.WriteLine($"TIMER START {MakeWatchDetailDescription()}");
        }

        public TimeSpan Stop()
        {
            // It is important that we don't put back the same stopwatch object multiple times on the object pool.  We don't
            // want duplicate entries in the pool.  The Interlocked.Exchange guards against multiple threads calling stop on the
            // same instance.  Only the first stop will be performed, the rest will return a timespan of 0 as the stopwatch is
            // considered stopped at that point.
            //
            var stopwatch = Interlocked.Exchange(ref _stopwatch, null);
            if (stopwatch == null || stopwatch.IsRunning == false)
                return TimeSpan.Zero;

            stopwatch.Stop();
            var totalRunningTime = stopwatch.Elapsed;
            stopwatch.Reset();

            _stopwatchObjectPool.PutObject(stopwatch);
            stopwatch = null;   // We have given up our reference to the stopwatch so don't use it anymore

            if (StopTimeWarning is TimeSpan stopTimeWarning && totalRunningTime > stopTimeWarning)
                Console.WriteLine($"TIMER STOPPED({totalRunningTime.TotalMilliseconds}ms > {stopTimeWarning.TotalMilliseconds}ms) {MakeWatchDetailDescription()}");
            else if (Option.HasFlag(PerformanceTimerOption.OnShowStopTotalTimeInMs))
                Console.WriteLine($"TIMER STOPPED({totalRunningTime.TotalMilliseconds}ms {MakeWatchDetailDescription()})");

            return totalRunningTime;
        }

        public void Mark(string message)
        {
            var stopwatch = _stopwatch;
            var totalRunningTime = stopwatch?.Elapsed;
            if (stopwatch is null || stopwatch.IsRunning is false) return;
            Console.WriteLine($"TIMER MARK({message} - {totalRunningTime}ms)");
        }

        /// <summary>
        /// Dispose is not required, but it allows for the using pattern.  However, when constructing a new PerformanceTimer be sure to specify auto start.  
        /// </summary>
        public void Dispose()
        {
            Stop();
        }
    }

}