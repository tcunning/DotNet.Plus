using DotNet.Plus.Pattern;
using DotNet.Plus.Pattern.BackgroundOperation;
using DotNet.Plus.Tasks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Plus.Time
{
    public class Watchdog : CommonDisposable
    {
        private readonly BackgroundOperation _backgroundOperation;

        public bool AutoStartOnFirstPet { get; }

        private TimeSpan _lastPetTime = TimeSpan.Zero;

        public bool IsMonitorStarted => _lastPetTime != TimeSpan.Zero;

        private readonly object _lock = new object();

        public bool IsCanceled { get; private set; }

        public TimeSpan PetTimeout { get; }
        
        public TimeSpan PetMaxTimeUntilTriggered { get; }

        private readonly Action? _triggerCallback;

        public bool IsTriggered { get; private set; } = false;

        private TaskCompletionSource<bool>? _triggerTcs = null;  // If requested, this will be completed if watchdog is triggered or disposed

        public Watchdog(TimeSpan petTimeout, bool autoStartOnFirstPet = false) :
            this(petTimeout, null, autoStartOnFirstPet)
        {
        }

        public Watchdog(TimeSpan petTimeout, Action? triggerCallback, bool autoStartOnFirstPet = false) :
            this(petTimeout, TimeSpan.Zero, triggerCallback, autoStartOnFirstPet)
        {
        }

        public Watchdog(TimeSpan petTimeout, TimeSpan petMaxTimeUntilTriggered, Action? triggerCallback, bool autoStartOnFirstPet)
        {
            AutoStartOnFirstPet = autoStartOnFirstPet;
            PetTimeout = petTimeout;
            PetMaxTimeUntilTriggered = petMaxTimeUntilTriggered;
            _triggerCallback = triggerCallback;
            _backgroundOperation = new BackgroundOperation(WatchdogOperationAsync);
        }

        private async Task WatchdogOperationAsync(CancellationToken cancellationToken)
        {
            IsTriggered = false;
            var firstPetTime = FreeRunningTimer.ElapsedTime;

            while( !cancellationToken.IsCancellationRequested && !IsDisposed )
            {
                var timeTaken = FreeRunningTimer.ElapsedTime - _lastPetTime;
                var sleepTime = (int)PetTimeout.TotalMilliseconds - (int)timeTaken.TotalMilliseconds;

                // if we are configured to have a max time until triggered, check to see if we need to adjust our sleep time to take into
                // account the max available time left.  We take whichever time is less and use it as our sleep time.  If sleepTime is already
                // less then 0 then we don't have to check as we know the watchdog needs to be triggered!
                //
                if( PetMaxTimeUntilTriggered > TimeSpan.Zero && sleepTime > 0 ) {
                    var maxSleepTime = (int)PetMaxTimeUntilTriggered.TotalMilliseconds - (int) (FreeRunningTimer.ElapsedTime - firstPetTime).TotalMilliseconds;
                    if( maxSleepTime < sleepTime )
                        sleepTime = maxSleepTime;
                }

                if( sleepTime <= 0 ) 
                    break;

                await sleepTime.TryDelay(cancellationToken);
            }

            lock(_lock)
            {
                if( IsDisposed ) {
                    _triggerTcs?.TrySetException(new WatchdogDisposedException());
                    return;
                }
                
                if( IsCanceled ) 
                    return;

                IsTriggered = true;
                try { _triggerCallback?.Invoke(); } catch { /* ignored */ }
                _triggerTcs?.TrySetResult(true);
            }
        }

        public void Monitor()
        {
            lock( _lock )
            {
                if( IsDisposed )
                    throw new WatchdogDisposedException();

                if( IsTriggered )
                    throw new WatchdogTriggeredException();

                if( IsMonitorStarted )
                    throw new WatchdogAlreadyStartedException();

                _lastPetTime = FreeRunningTimer.ElapsedTime;  // We consider the monitor start to be the "first" pet
                _backgroundOperation.Start();
            }
        }

        public void Cancel()
        {
            lock( _lock )
            {
                if( IsDisposed )
                    throw new WatchdogDisposedException();

                if( IsTriggered )
                    throw new WatchdogTriggeredException();

                if( !IsMonitorStarted )
                    throw new WatchdogNotStartedException();

                IsCanceled = true;
                _triggerTcs?.TrySetException(new WatchdogCanceledException());
                _backgroundOperation.Stop();
            }
        }
        
        public TimeSpan Pet(bool autoReset = false)
        {
            lock( _lock )
            {
                if( IsDisposed )
                    throw new WatchdogDisposedException();

                if( !IsMonitorStarted )
                {
                    if( !AutoStartOnFirstPet )
                        throw new WatchdogNotStartedException();

                    Monitor();
                    return TimeSpan.Zero;
                }

                if( IsCanceled || IsTriggered )
                {
                    if( !autoReset )
                        throw IsTriggered ? (Exception)new WatchdogTriggeredException() : new WatchdogCanceledException();

                    IsCanceled = false;
                    IsTriggered = false;
                    _lastPetTime = TimeSpan.Zero;

                    // If we have a trigger task that is completed, we need to reset it (by making it null).  The caller
                    // is responsible for getting the new task, if needed, from AsTask.
                    //
                    if( _triggerTcs?.Task.IsCompleted ?? false )
                        _triggerTcs = null;

                    _backgroundOperation.Stop();
                    Monitor();
                    return TimeSpan.Zero;
                }

                // Pet it
                //
                var oldLastPetTime = _lastPetTime;
                _lastPetTime = FreeRunningTimer.ElapsedTime;

                var elapsed = _lastPetTime - oldLastPetTime;
                return elapsed;
            }
        }
        
        /// <summary>
        /// This returns a task that will be completed iff the Watchdog is trigger or if the Watchdog is disposed.  If
        /// this is called multiple times the same Task will be returned (UNLESS THE WATCHDOG HAS BEEN RESET). When doing
        /// an autoReset Pet on the watchdog, a new Task may be returned when the watchdog is reset.
        ///
        /// The returned task will be in a Exception state if the Watchdog was disposed w/o being triggered.
        /// </summary>
        /// <returns>A task that will be completed when the watchdog is triggered or disposed.</returns>
        public Task<bool> AsTask()
        {
            lock( _lock )
            {
                if( _triggerTcs == null ) {
                    if( IsDisposed )
                        return Task.FromException<bool>(new WatchdogDisposedException());

                    if( IsCanceled )
                        return Task.FromException<bool>(new WatchdogCanceledException());

                    if( IsTriggered )
                        return Task.FromResult(true);

                    _triggerTcs = new TaskCompletionSource<bool>();
                }

                return _triggerTcs.Task;
            }
        }

        protected override void Dispose(bool disposing)
        {
            lock( _lock )
            {
                if( IsMonitorStarted && !IsTriggered )
                    IsCanceled = true;

                _triggerTcs?.TrySetException(new WatchdogDisposedException());

                _backgroundOperation.Stop();
            }
        }
    }

}
