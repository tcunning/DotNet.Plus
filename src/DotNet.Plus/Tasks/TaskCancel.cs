using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Plus.Tasks
{
    public static class TaskCancel
    {
        public static async Task<TResult> CancelWhen<TResult>(this Task<TResult> task, CancellationToken cancellationToken, int timeoutMs)
        {
            using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            if( timeoutMs > 0 )
                cancellationTokenSource.CancelAfter(timeoutMs);

            var cancellationTcs = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            await Task.WhenAny(task, cancellationTcs.Task);

            return task.Result;
        }

        public static Task<TResult> CancelWhen<TResult>(this TaskCompletionSource<TResult> tcs, int timeoutMs) =>
            CancelWhen(tcs, CancellationToken.None, timeoutMs);
        
        public static async Task<TResult> CancelWhen<TResult>(this TaskCompletionSource<TResult> tcs, CancellationToken cancellationToken, int timeoutMs = Timeout.Infinite)
        {
            using var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            if( timeoutMs > 0 )
                cancellationTokenSource.CancelAfter(timeoutMs);

            TaskCompletionSource<TResult> cancellationTcs = new TaskCompletionSource<TResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            void UpdateWhenCanceledOrTimedOut() {
                if( cancellationToken.IsCancellationRequested )
                    cancellationTcs.SetCanceled();
                else // We must have timed out
                    cancellationTcs.SetException(new TimeoutException($"Timed out after {timeoutMs}ms"));
            }

            using( cancellationTokenSource.Token.Register(UpdateWhenCanceledOrTimedOut) )
            {
                // WhenAny will not throw, unless its arguments are invalid.
                //
                await Task.WhenAny(tcs.Task, cancellationTcs.Task);

                // If the given tcs was completed then go ahead and use its results.  It will throw if the tcs was
                // in an exception or canceled state.
                //
                if( tcs.Task.IsCompleted )
                    return tcs.Task.Result;  // This will throw the tcs.Task.Exception or Canceled Exception if needed

                // If our cancellation tcs is in an exception state use it.  It will be in an exception state if the
                // operation timed out.
                //
                if( cancellationTcs.Task.Exception != null ) {
                    tcs.TrySetException(cancellationTcs.Task.Exception);
                    throw cancellationTcs.Task.Exception;
                }

                // If we got here, the only other choice is the operation was canceled
                //
                tcs.TrySetCanceled();
                throw new TaskCanceledException();
            }
        }

    }
}
