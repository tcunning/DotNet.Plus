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

        private TimeSpan _lastPetTime = TimeSpan.Zero;

        private readonly object _lock = new object();

        private readonly Action? _triggerCallback;

        private TaskCompletionSource<bool>? _triggerTcs = null;  // If requested, this will be completed if watchdog is triggered or disposed

        /// <summary>
        /// True if the watchdog is configured to automatically start on the first pet.  If this isn't true, then the
        /// <see cref="Monitor"/> method must be called to start the watchdog.
        /// </summary>
        public bool AutoStartOnFirstPet { get; }

        /// <summary>
        /// True if the monitor has been started, otherwise false.
        /// </summary>
        public bool IsMonitorStarted => _lastPetTime != TimeSpan.Zero;

        /// <summary>
        /// True if the Watchdog has been canceled.  It can be restarted if a <see cref="Pet"/> is done with the auto
        /// reset option.
        /// </summary>
        public bool IsCanceled { get; private set; }

        /// <summary>
        /// If a <see cref="Pet"/> hasn't been performed within this time span, the watchdog will be triggered.
        /// </summary>
        public TimeSpan PetTimeout { get; }
        
        /// <summary>
        /// If this is non-zero, it specifies the maximum amount of time the Watchdog can go without being triggered,
        /// even if it is being <see cref="Pet"/>.  This is useful to prevent starvation.
        /// </summary>
        public TimeSpan PetMaxTimeUntilTriggered { get; }
        
        /// <summary>
        /// True if the watchdog is triggered because a <see cref="Pet"/> didn't happen soon enough or the PetMaxTimeUntilTriggered time
        /// was reached.  This can be cleared if a <see cref="Pet"/> is performed with the auto reset option.
        /// </summary>
        public bool IsTriggered { get; private set; } = false;

        /// <summary>
        /// Creates a new Watchdog with a timeout and an optional auto start on first pet.
        /// </summary>
        /// <param name="petTimeout">Once started, the watchdog must be <see cref="Pet"/> within this time span of it will be triggered</param>
        /// <param name="autoStartOnFirstPet">If true, the watchdog will automatically be started on first <see cref="Pet"/>.
        /// Otherwise, <see cref="Monitor"/> has to be called to start the watchdog.</param>
        public Watchdog(TimeSpan petTimeout, bool autoStartOnFirstPet = false) :
            this(petTimeout, null, autoStartOnFirstPet)
        {
        }

        /// <summary>
        /// Creates a new Watchdog with a timeout, trigger lambda, and an optional auto start on first pet.
        /// </summary>
        /// <param name="petTimeout">Once started, the watchdog must be <see cref="Pet"/> within this time span of it will be triggered</param>
        /// <param name="triggerCallback">This lambda is called when/if the watchdog is triggered.</param>
        /// <param name="autoStartOnFirstPet">If true, the watchdog will automatically be started on first <see cref="Pet"/>.
        /// Otherwise, <see cref="Monitor"/> has to be called to start the watchdog.</param>
        public Watchdog(TimeSpan petTimeout, Action? triggerCallback, bool autoStartOnFirstPet = false) :
            this(petTimeout, TimeSpan.Zero, triggerCallback, autoStartOnFirstPet)
        {
        }

        /// <summary>
        /// This is the primary constructor for the Watchdog that allows all options to be specified.
        /// </summary>
        /// <param name="petTimeout">Once started, the watchdog must be <see cref="Pet"/> within this time span of it will be triggered</param>
        /// <param name="petMaxTimeUntilTriggered">Once the watchdog is started, it will be triggered after this time span even if
        /// <see cref="Pet"/> is called.</param>
        /// <param name="triggerCallback">This lambda is called when/if the watchdog is triggered.</param>
        /// <param name="autoStartOnFirstPet">If true, the watchdog will automatically be started on first <see cref="Pet"/>.
        /// Otherwise, <see cref="Monitor"/> has to be called to start the watchdog.</param>
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

        /// <summary>
        /// This starts the watchdog iff it isn't already started
        /// </summary>
        /// <exception cref="WatchdogDisposedException">Watchdog has been disposed</exception>
        /// <exception cref="WatchdogTriggeredException">Watchdog has been triggered, use <see cref="Pet"/> with the autoRestart option to restart it</exception>
        /// <exception cref="WatchdogAlreadyStartedException">Watchdog has already been started</exception>
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

        /// <summary>
        /// This cancels the watchdog iff it is current started and not triggered
        /// </summary>
        /// <exception cref="WatchdogDisposedException">Watchdog has been disposed</exception>
        /// <exception cref="WatchdogTriggeredException">Watchdog has been triggered so it can't be canceled</exception>
        /// <exception cref="WatchdogNotStartedException">Watchdog hasn't been started so there is nothing to cancel</exception>
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

        /// <summary>
        /// Pet's the watchdog to keep it from triggering.  If pet isn't called within the watchdog's PetTimeout, the watchdog
        /// will be triggered.  
        /// </summary>
        /// <param name="autoReset">If true, the watchdog will be auto reset if it has already been triggered.</param>
        /// <exception cref="WatchdogDisposedException">Watchdog has been disposed</exception>
        /// <exception cref="WatchdogTriggeredException">Watchdog has been triggered and autoReset is false</exception>
        /// <exception cref="WatchdogNotStartedException">Watchdog hasn't been started and AutoStartOnFirstPet not set</exception>
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
        /// an autoReset Pet on the watchdog, a new Task may be returned when the watchdog is reset from a triggered state.
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
