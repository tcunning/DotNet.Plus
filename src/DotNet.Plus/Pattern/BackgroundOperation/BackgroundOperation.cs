using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Plus.Pattern.BackgroundOperation
{
    public class BackgroundOperation : BackgroundOperationBase, IBackgroundOperation
    {
        public delegate Task BackgroundOperationFunc(CancellationToken cancellationToken);
        public delegate void BackgroundOperationAction(CancellationToken cancellationToken);

        private readonly BackgroundOperationFunc? _backgroundOperation;

        /// <summary>
        /// Derived classes may use this constructor, but if they do so they should override BackgroundOperationAsync to provide the background action.  The
        /// default constructor is not intended for non-derived classes as they would have no way to specify the background action!
        /// </summary>
        protected BackgroundOperation()
        {
            _backgroundOperation = null;
        }

        /// <summary>
        /// Setups up a background action with a task.  
        /// </summary>
        /// <param name="operation">The background operation to be performed when start is called.  The operation must return when the cancellation token is canceled.</param>
        public BackgroundOperation(BackgroundOperationFunc operation) : this()
        {
            _backgroundOperation = operation ?? throw new ArgumentNullException(nameof(operation));
        }

        /// <summary>
        /// Setups up a background action with a simple action.  
        /// </summary>
        /// <param name="action">The background action to be performed when start is called.  The action must return when the cancellation token is canceled.</param>
        public BackgroundOperation(BackgroundOperationAction action) : this()
        {
            if( action == null )
                throw new ArgumentNullException(nameof(action));

            _backgroundOperation = (cancelToken) =>
            {
                action?.Invoke(cancelToken);
                return Task.CompletedTask;
            };
        }

        /// <summary>
        /// This is used for derived classes to provide any easy way to provide the BackgroundOperationAsync.  When overridden, the base implementation of this
        /// method doesn't need to be invoked as _backgroundOperation will be null!
        /// </summary>
        /// <param name="cancellationToken"></param>
        protected virtual async Task BackgroundOperationAsync(CancellationToken cancellationToken)
        {
            if( _backgroundOperation == null )
                return;
            await _backgroundOperation.Invoke(cancellationToken);
        }

        /// <summary>
        /// Override the base class implementation so that we can 
        /// </summary>
        /// <param name="args"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override Task BackgroundOperationAsync(object[]? args, CancellationToken cancellationToken) => BackgroundOperationAsync(cancellationToken);

        /// <summary>
        /// Starts the background action.  If the action  is already running, the task will be marked for restart (unless stop gets called).
        /// This allows for rapid start/stop sequences where the last one called is the state the background action will end up in.
        /// 
        /// STOP WILL ALWAYS STOP THE CURRENT TASK AND ANY *PENDING* RESTARTS OF THE TASK.  Note: this is not a reference counting system.
        /// If stop is called, the background action will be stopped.
        ///
        /// If start is called multiple times, only the arguments given to the first start will be used until a stop is called.  So to make sure
        /// the last given start arguments are used call STOP following by the intended START arguments.  This will guarantee the last given start
        /// arguments are used. 
        /// </summary>
        public virtual void Start() => BackgroundOperationStart(args: null);

        /// <summary>
        /// Stop's the background action and any pending restarts of the background action.
        /// </summary>
        public virtual void Stop() => BackgroundOperationStop();
    }
}
