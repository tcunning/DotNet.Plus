using System;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Plus.Core;
using DotNet.Plus.Pattern;

namespace DotNet.Plus.Tasks
{
    public class TaskLockTracker : CommonDisposable
    {
        private readonly TaskLock _taskLock;

        private readonly CancellationTokenSource _cts;

        internal CancellationToken CancelToken { get; }

        private readonly TaskCompletionSource<bool> _lockGrantedTcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        internal Task WaitForLockAsync() => _lockGrantedTcs.CancelWhen(CancelToken);

        internal void GrantLock() => _lockGrantedTcs.TrySetResult(true);

        internal TaskLockTracker(TaskLock taskLock, CancellationToken cancelToken, TimeSpan timeout)
        {
            _taskLock = taskLock;

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancelToken);
            if( timeout < TimeSpan.MaxValue )
                _cts.CancelAfter(timeout);
            CancelToken = _cts.Token;
        }

        public override void Dispose(bool disposing)
        {
            _taskLock.LockNoLongerNeeded(this);
            _cts.TryCancelAndDispose();
        }
    }
}
