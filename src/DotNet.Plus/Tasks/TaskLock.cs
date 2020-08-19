using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DotNet.Plus.Collection;

namespace DotNet.Plus.Tasks
{
    public class TaskLock
    {
        private readonly List<TaskLockTracker> _lockedTaskQueue = new List<TaskLockTracker>();

        private TaskLockTracker? _currentLock = null;

        public int MaxQueueSize { get; }

        public TaskLock(int maxQueueSize = Int32.MaxValue)
        {
            MaxQueueSize = maxQueueSize;
        }

        public Task<TaskLockTracker> GetLock(CancellationToken cancelToken) => GetLock(cancelToken, TimeSpan.MaxValue);

        ///<summary>
        ///<para>Enables sequential execution by only allowing one lock to be acquired at a time.  The locks will be processed in
        ///FIFO order.</para>
        ///<para>NOTE: The caller MUST dispose the returned object in order to release the lock and allows others to get a
        /// chance to execute.</para>
        /// </summary>
        ///<example>
        ///<code>
        /// TaskSerialQueue _pidSerialQueue = new TaskSerialQueue(maxQueueSize: 100);
        ///
        /// using(await _pidSerialQueue.GetLock(cancellationToken))
        /// {
        ///     /* This code will be done under the acquired lock, only one lock will be given out at a time */
        /// }
        /// </code>
        /// </example>
        /// <param name="cancelToken">The GetLock will throw a cancellation exception if this token is canceled before the lock is granted</param>
        /// <param name="timeout">The GetLock will throw a timeout exception if this timeout occurs before the lock is granted</param>
        /// <returns>An exception or the TaskSerialLock which should be disposed by the caller when the lock is no longer needed</returns>
        public async Task<TaskLockTracker> GetLock(CancellationToken cancelToken, TimeSpan timeout)
        {
            var serialLock = new TaskLockTracker(this, cancelToken, timeout);

            lock( _lockedTaskQueue )
            {
                // If we don't have room in the queue then fail
                //
                if( _lockedTaskQueue.Count > MaxQueueSize ) 
                    throw new IndexOutOfRangeException($"GetLock: failed because maximum queue Size of {MaxQueueSize} reached"); ;

                // If we have no current lock then there is nothing queued so we can just go ahead get the lock now; otherwise,
                // we just add our lock request to the queue.
                //
                if( _currentLock == null ) {
                    _currentLock = serialLock;
                    _currentLock.GrantLock();
                    return serialLock;
                }

                _lockedTaskQueue.Add(serialLock);
            }

            try
            {
                await serialLock.WaitForLockAsync();
                return serialLock;
            }
            catch
            {
                // Make sure the lock has been properly disposed as we got an exception in processing the lock.
                // The using pattern won't know to call dispose because an exception occurred so we do it here.
                //
                serialLock.TryDispose();
                throw;
            }
        }

        internal void LockNoLongerNeeded(TaskLockTracker lockTracker)
        {
            // NOTE: We do not need to dispose the _currentLock because LockNoLongerNeeded is only called
            //       by the dispose method of TaskLockTracker so we count on it already being disposed.
            //
            lock( _lockedTaskQueue )
            {
                // If the given serialLock doesn't match the _currentLock then go ahead and remove it from
                // the lock list as it no longer requires a lock.
                //
                if( !object.ReferenceEquals(_currentLock, lockTracker) )
                {
                    _lockedTaskQueue.TryRemove(lockTracker);
                    return;
                }

                // The given lockTracker is equal to the current lock so go ahead and release the current lock
                // and find the next one available.
                //
                _currentLock = null;

                // Take the first lock we find and make it the current lock
                //
                while( _lockedTaskQueue.TryTakeFirst(out var nextLock) )
                {
                    // If the next lock is canceled or timed out, we just skip it.
                    //
                    if( nextLock.IsDisposed || nextLock.CancelToken.IsCancellationRequested )
                        continue;

                    // We found a lock request ready so mark it as our current lock request.
                    //
                    _currentLock = nextLock;
                    nextLock.GrantLock();
                    return;
                }

                /* No locks are waiting */
            }
        }

    }
}
