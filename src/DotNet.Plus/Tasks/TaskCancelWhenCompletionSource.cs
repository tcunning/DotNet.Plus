using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Plus.Tasks
{
    /// <summary>
    /// Allows an existing TaskCompletionSource to be canceled when a given Cancellation Token is canceled or timed out when
    /// a timeout time is reached.
    /// </summary>
    public static class TaskCancelWhenCompletionSource
    {
        /// <summary>
        /// Cancels the given task completion source on timeout unless the tcs gets completed.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="tcs">The Task Completion Source, this will be canceled and/or failed with a timeout exception if
        /// one of those occurs.</param>
        /// <param name="timeoutMs">An optional timeout, the value Timeout.Infinite can be specified for no timeout</param>
        /// <returns>A task that can be awaited to get the completion result</returns>
        /// <exception cref="TimeoutException">This will be thrown if the tcs times out OR if timeoutMs is exceeded</exception>
        public static Task<TResult> CancelWhen<TResult>(this TaskCompletionSource<TResult> tcs, int timeoutMs) =>
            CancelWhen(tcs, CancellationToken.None, timeoutMs);

        /// <summary>
        /// Cancels the given task completion source when the given cancellationToken is canceled and/or on timeout unless
        /// the tcs gets completed.
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="tcs">The Task Completion Source, this will be canceled and/or failed with a timeout exception if
        /// one of those occurs.</param>
        /// <param name="cancellationToken">A cancellation token or CancellationToken.None</param>
        /// <param name="timeoutMs">An optional timeout, the value Timeout.Infinite can be specified for no timeout</param>
        /// <returns>A task that can be awaited to get the completion result</returns>
        /// <exception cref="TaskCanceledException">This will be thrown if the tcs is canceled OR the cancellationToken is canceled</exception>
        /// <exception cref="TimeoutException">This will be thrown if the tcs times out OR if timeoutMs is exceeded</exception>
        public static async Task<TResult> CancelWhen<TResult>(this TaskCompletionSource<TResult> tcs, CancellationToken cancellationToken, int timeoutMs = Timeout.Infinite)
        {
            if( cancellationToken == CancellationToken.None && timeoutMs < 0 )
                return await tcs.Task.ConfigureAwait(false);

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
                await Task.WhenAny(tcs.Task, cancellationTcs.Task).ConfigureAwait(false);

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
