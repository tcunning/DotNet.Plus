using System.Threading;

namespace DotNet.Plus.Pattern.BackgroundOperation
{
    /// <summary>
    /// This is based on BackgroundOperation, but provides a Disposable version that can be used to stop the task automatically
    /// when the BackgroundOperationDisposable is disposed.
    /// </summary>
    public class BackgroundOperationDisposable : BackgroundOperation, ICommonDisposable
    {
        private const string LogTag = nameof(BackgroundOperationDisposable);

        /// <summary>
        /// Derived classes may use this constructor, but if they do so they should override BackgroundOperationAsync to provide the background action.  The
        /// default constructor is not intended for non-derived classes as they would have no way to specify the background action!
        /// </summary>
        protected BackgroundOperationDisposable() : base()
        {
        }

        /// <summary>
        /// Setups up a background action with a task.  
        /// </summary>
        /// <param name="operation">The background operation to be performed when start is called.  The operation must return when the cancellation token is canceled.</param>
        public BackgroundOperationDisposable(BackgroundOperationFunc operation) : base(operation)
        {
        }

        /// <summary>
        /// Setups up a background action with a simple action.  
        /// </summary>
        /// <param name="action">The background action to be performed when start is called.  The action must return when the cancellation token is canceled.</param>
        public BackgroundOperationDisposable(BackgroundOperationAction action) : base(action)
        {
        }

        /// <summary>
        /// Starts the background action.  If the action  is already running, the task will be marked for restart (unless stop gets called).
        /// This allows for rapid start/stop sequences where the last one called is the state the background action will end up in.
        /// 
        /// STOP WILL ALWAYS STOP THE CURRENT TASK AND ANY *PENDING* RESTARTS OF THE TASK.  Note: this is not a reference counting system.
        /// If stop is called, the background action will be stopped.
        /// </summary>
        public override void Start()
        {
            lock( Locker )
            {
                // Once disposed, we don't allow the task to be started again and we make sure we are in the stop state.
                if( IsDisposed )
                {
                    Stop();
                    return;
                }

                base.Start();
            }
        }


        #region ICommonDisposable
        private int _isDisposed;

        public bool IsDisposed => (uint)_isDisposed > 0U;

        public void TryDispose()
        {
            try
            {
                if( IsDisposed )
                    return;
                Dispose();
            }
            catch { }
        }

        public void Dispose()
        {
            if( this.IsDisposed || Interlocked.Exchange(ref this._isDisposed, 1) != 0 )
                return;
            this.Dispose(true);
        }

        public virtual void Dispose(bool disposing)
        {
            Stop();
        }
        #endregion
    }
}
