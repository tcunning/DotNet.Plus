using System;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Plus.Core;
using DotNet.Plus.Pattern;

namespace DotNet.Plus.Tasks
{
    /// <summary>
    /// This class is created and used by <see cref="TaskLock"/> in order to manage locks that have been created.
    /// It is important that the recipient calls dispose when they are done with the lock to allow other access.
    /// </summary>
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
            if( timeout > TimeSpan.Zero && timeout < TimeSpan.MaxValue)
                _cts.CancelAfter(timeout);  // todo: would be nice if we threw a TimeoutException vs TaskCanceledException
            CancelToken = _cts.Token;
        }

        protected override void Dispose(bool disposing)
        {
            _taskLock.LockNoLongerNeeded(this);
            _cts.TryCancelAndDispose();
        }
    }
}
