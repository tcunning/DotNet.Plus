using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Plus.Pattern.BackgroundOperation
{
    /// <summary>
    /// This class provides a reusable controller for safely supporting the starting and stopping of a background service.  It allows
    /// for multiple start and stops be performed asynchronously, with the last one performed being the end state.
    /// For example:
    ///  
    ///     Start - Starts background operation
    ///     Stop - Stops the background operation (it may take a short period of time for the action to finally be stopped)
    ///     Start - If the operation is stopped it will be immediately started, If stop hasn't finished from a previous stop  
    ///             then the operation will be restarted as soon as it has finished stopping.
    ///  
    /// This class doesn't allow 2 instances of the operation to running at the same time.  It GUARANTEES that
    /// at most only one instance of the operation will be running any given time.  This helps simplify the async logic of the
    /// background operation.  It knows there will be at most ONE instance of itself running at a time.
    /// </summary>
    public abstract class BackgroundOperationService<TSingleton> : Singleton<BackgroundOperationService<TSingleton>>, IBackgroundOperation
        where TSingleton : class
    {
        private readonly BackgroundOperation _backgroundOperation;

        public bool Started => _backgroundOperation.Started;

        public bool StartedOrWillStart => _backgroundOperation.StartedOrWillStart;

        protected BackgroundOperationService()
        {
            _backgroundOperation = new BackgroundOperationDisposable(BackgroundOperationAsync);
        }

        public virtual void Start() => _backgroundOperation.Start();

        public virtual void Stop() => _backgroundOperation.Stop();

        protected abstract Task BackgroundOperationAsync(CancellationToken cancellationToken);
    }

}

