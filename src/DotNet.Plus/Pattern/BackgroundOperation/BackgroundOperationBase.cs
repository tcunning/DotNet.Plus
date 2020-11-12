using DotNet.Plus.Core;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Plus.Pattern.BackgroundOperation
{
    public abstract class BackgroundOperationBase
    {
        public bool Started { get; private set; }

        protected readonly object Locker = new object();

        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

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


        protected BackgroundOperationBase()
        {
            Started = false;
        }

        /// <summary>
        /// The background operation to perform
        /// </summary>
        /// <param name="args">Arguments may be null or an empty array when there are no arguments.</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected abstract Task BackgroundOperationAsync(object[]? args, CancellationToken cancellationToken);

        private bool _restartRequested;
        private object[]? _restartArgs;

        /// <summary>
        /// <para>Starts or restarts the background operation.  It provides for the following safeguards:
        /// <list type="bullet">
        ///<item><description>The background operation will end up in the state of whichever is called LAST: Start or Stop.</description></item>
        ///<item><description>Ensures that a previous background operation must is fully stopped before it will be restarted.</description></item>
        ///<item><description>If the background operation is in the process of Stopping and Start is called, the operation will get restarted</description></item>
        ///<item><description>Stop can cancel a restart</description></item>
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
