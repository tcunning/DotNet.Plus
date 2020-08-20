using System;
using System.Threading;

namespace DotNet.Plus.Core
{
    public static class CancellationTokenSourceEx
    {
        /// <summary>
        /// Will cancel the token source if and only if it hasn't already been canceled.
        /// </summary>
        /// <param name="cts">The token source to cancel</param>
        public static void TryCancel(this CancellationTokenSource? cts)
        {
            Operation.TryCatch(() => {
                if( cts != null && !cts.IsCancellationRequested )
                    cts.Cancel();
            });
        }

        /// <summary>
        /// Will cancel the token source if and only if it hasn't already been canceled.  And then
        /// the object will be disposed  Exceptions from the Cancel or Dispose process will be ignored.
        /// </summary>
        /// <param name="cts">CancellationTokenSource</param>
        public static void TryCancelAndDispose(this CancellationTokenSource? cts)
        {
            Operation.TryCatch(() => {
                cts?.TryCancel();
                cts?.Dispose();
            });
        }
    }
}
