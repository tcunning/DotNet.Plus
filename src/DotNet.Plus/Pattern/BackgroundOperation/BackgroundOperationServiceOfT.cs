using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Plus.Pattern.BackgroundOperation
{
    public abstract class BackgroundOperationService<TSingleton, TArg1> : Singleton<BackgroundOperationService<TSingleton, TArg1>>, IBackgroundOperation<TArg1>
        where TSingleton : class
    {
        private readonly BackgroundOperation<TArg1> _backgroundOperation;

        public bool Started => _backgroundOperation.Started;

        public bool StartedOrWillStart => _backgroundOperation.StartedOrWillStart;

        protected BackgroundOperationService()  /* Required for Singleton */
        {
            _backgroundOperation = new BackgroundOperation<TArg1>(BackgroundOperationAsync);
        }

        public virtual void Start(TArg1 arg1) => _backgroundOperation.Start(arg1);

        public virtual void Stop() => _backgroundOperation.Stop();

        protected abstract Task BackgroundOperationAsync(TArg1 arg1, CancellationToken cancellationToken);
    }

}

