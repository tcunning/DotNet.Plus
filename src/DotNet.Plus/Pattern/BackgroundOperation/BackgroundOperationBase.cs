using DotNet.Plus.Core;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Plus.Pattern.BackgroundOperation
{
    public abstract class BackgroundOperationBase
    {
        /// <summary>
        /// True if the background operation has been started and is currently running.
        /// 
        /// There can be a delay between the background operation being started and this being true because it can take time to
        /// start the background task/operation.  Thus starting the background operation and then immediately calling this
        /// may result is false as the task has not yet started or had a chance to set its started state.
        /// </summary>
        public bool Started { get; private set; }

        /// <summary>
        /// Can be used as a sync-lock with starting/stopping the background operation by derived classes.
        /// </summary>
        protected readonly object Locker = new object();
        
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// True if the background operation has been started and is currently running OR will automatically be restarted when
        /// it's stopped.
        ///
        /// There can be a delay between the background operation being started and this being true because it can take time to
        /// start the background task/operation.  Thus starting the background operation and then immediately calling this
        /// may result is false as the task has not yet started or had a chance to set its started state.
        /// </summary>
        public bool StartedOrWillStart
        {
            get
            {
                lock( Locker )
                {
                    return Started || _restartRequested;
                }
            }
        }

        /// <summary>
        /// Default constructor that sets the Started state to false;
        /// </summary>
        protected BackgroundOperationBase()
        {
            Started = false;
        }

        /// <summary>
        /// The background operation to perform.
        ///
        /// This must be implemented by base classes to implement the operation. The internal Started state will be
        /// managed by <see cref="BackgroundOperationStart"/> so the background operation can focus only on what it
        /// needs to get done.
        ///
        /// When the task completes for any reason, the background operation will be automatically stopped and/or
        /// restarted (if requested).
        /// 
        /// Exceptions thrown by the Background Operation will be ignored and cause the Task to be completed.
        /// </summary>
        /// <param name="args">Arguments may be null or an empty array when there are no arguments.</param>
        /// <param name="cancellationToken">The background operation should return as soon as possible the the given
        /// cancellation token is canceled</param>
        /// <returns>The Background Operation Task</returns>
        protected abstract Task BackgroundOperationAsync(object[]? args, CancellationToken cancellationToken);

        private bool _restartRequested;
        private object[]? _restartArgs;

        /// <summary>
        /// <para>Starts or restarts the background operation.  It provides for the following safeguards:
        /// <list type="bullet">
        ///<item><description>The background operation will end up in the state of whichever is called LAST: Start or Stop.</description></item>
        ///<item><description>Ensures that a previous background operation must is fully stopped before it will be restarted.</description></item>
        ///<item><description>If the background operation is in the process of Stopping and Start is called, the operation will get restarted</description></item>
        ///<item><description>Stop can/will cancel a restart</description></item>
        ///<item><description>If the background operation naturally finishes or throws, the background operation will be considered STOPPED</description></item>
        /// </list></para>
        /// <para>NOTE: The background operation won't be restarted if it naturally exists or throws an exception at the same time Start was called.  This is
        /// considered a race condition because the operation "naturally" finishing or threw at the "Same" time Start was called.  When this happen, the natural
        /// finish is favored over the Start.  This only applies to background operations that naturally stop or throw.  If the operation is stopped because Stop
        /// is called, then one can be sure the operation will be started or restarted if Start is called.</para>
        /// </summary>
        /// <param name="args"></param>
        protected virtual void BackgroundOperationStart(object[]? args)
        {
            CancellationToken cancelToken;

            lock( Locker )
            {
                if( Started ) {
                    _restartRequested = true;
                    _restartArgs = args;
                    return;
                }

                _restartRequested = false;
                Started = true;

                _cancellationTokenSource?.TryCancelAndDispose();
                _cancellationTokenSource = new CancellationTokenSource();
                cancelToken = _cancellationTokenSource.Token;
            }

            Task.Run(async () =>
            {
                try
                {
                    if( cancelToken.IsCancellationRequested )
                        return;
                    await BackgroundOperationAsync(args, cancelToken);
                }
                catch { /* ignored */ }
                finally
                {
                    lock( Locker )
                    {
                        Started = false;
                        if( cancelToken.IsCancellationRequested && _restartRequested )
                            BackgroundOperationStart(_restartArgs);
                    }
                }
            }, CancellationToken.None); /* It's really important that we do not cancel the starting of Task.Run so we can cleanup our started state if we are canceled */
        }

        /// <summary>
        /// Stop's the background action and any pending restarts of the background action.
        /// </summary>
        protected virtual void BackgroundOperationStop()
        {
            lock( Locker )
            {
                // If we don't have a restart pending, then go ahead and release our reference to it as we don't need it anymore
                //
                if( !_restartRequested )
                    _restartArgs = default;

                _restartRequested = false;
                _cancellationTokenSource?.TryCancelAndDispose();
            }
        }
    }
}
