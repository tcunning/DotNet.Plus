using System;

namespace DotNet.Plus.Time
{
    public class WatchdogException : Exception
    {
        public WatchdogException(string message, Exception? innerException = null) :
            base(message, innerException)
        {
        }
    }

    public class WatchdogDisposedException : ObjectDisposedException
    {
        public WatchdogDisposedException(Exception? innerException = null) : base("The Watchdog has been disposed", innerException)
        {
        }
    }

    public class WatchdogTriggeredException : WatchdogException
    {
        public WatchdogTriggeredException(Exception? innerException = null) : base("The Watchdog has been triggered", innerException)
        {
        }
    }

    public class WatchdogNotStartedException : WatchdogException
    {
        public WatchdogNotStartedException(Exception? innerException = null) : base("The Watchdog hasn't been started", innerException)
        {
        }
    }

    public class WatchdogAlreadyStartedException : WatchdogException
    {
        public WatchdogAlreadyStartedException(Exception? innerException = null) : base("The Watchdog has already been started", innerException)
        {
        }
    }

    public class WatchdogCanceledException : WatchdogException
    {
        public WatchdogCanceledException(Exception? innerException = null) : base("The Watchdog has been canceled", innerException)
        {
        }
    }

}
