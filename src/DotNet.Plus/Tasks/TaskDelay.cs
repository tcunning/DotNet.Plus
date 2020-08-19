using System;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Plus.Core;

namespace DotNet.Plus.Tasks
{
    public static class TaskDelay
    {
        /// <summary>
        /// A version of Task.Delay that takes a timeSpan.
        /// </summary>
        /// <param name="timeSpan">The time to delay expressed as a TimeSpan</param>
        /// <param name="cancelToken">A cancellation token that will cancel the delay early</param>
        /// <returns>When the timeSpan completes or the operation is canceled</returns>
        public static Task Delay(TimeSpan timeSpan, CancellationToken cancelToken) => 
            Task.Delay((int)timeSpan.TotalMilliseconds, cancelToken);

        /// <summary>
        /// The result Delay version will throw when it is canceled, this version ignores all exceptions.
        /// </summary>
        /// <param name="timeMs">he time to delay expressed in milliseconds</param>
        /// <param name="cancelToken">A cancellation token that will cancel the delay early</param>
        /// <returns>true if the delay completed successfully, otherwise false is returned such as
        /// when the delay was canceled.</returns>
        public static Task<bool> TryDelay(int timeMs, CancellationToken cancelToken) =>
            Task.Delay(timeMs, cancelToken).TryCatchAsync(successValue: true);

        /// <summary>
        /// Helper method to delay w/o having to be concerned about handling exceptions.
        /// </summary>
        /// <param name="timespan"></param>
        /// <param name="cancelToken"></param>
        /// <returns>returns true if the delay completed successfully, otherwise false is returned.  The delay can fail because of the given cancellation token.</returns>
        public static Task<bool> TryDelay(TimeSpan timespan, CancellationToken cancelToken) =>
            Task.Delay(timespan, cancelToken).TryCatchAsync(successValue: true);

    }
}
