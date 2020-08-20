using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;

namespace DotNet.Plus.Tasks.Tests
{
    [TestClass]
    public class TaskLockTests
    {
        [TestMethod]
        [Timeout(5000)]
        public async Task GetLockTestAsync()
        {
            TaskLock lockQueue = new TaskLock(maxQueueSize: 100);

            var task1 = Task.Run(async () =>
            {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    await TaskDelay.TryDelay(2, CancellationToken.None);
                }
            });

            using( await lockQueue.GetLock(CancellationToken.None) )
            {
                await TaskDelay.TryDelay(2, CancellationToken.None);
            }

            await task1;
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task GetLockTest2Async()
        {
            TaskLock lockQueue = new TaskLock(maxQueueSize: 100);

            var startTcs = new TaskCompletionSource<bool>();
            var stopTcs = new TaskCompletionSource<bool>();
            
            var task1 = Task.Run(async () => {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    startTcs.TrySetResult(true);
                    await stopTcs.Task;
                }
            });

            try
            {
                await startTcs.Task;

                using var preCts = new CancellationTokenSource();
                preCts.Cancel();
                using (await lockQueue.GetLock(preCts.Token)) {
                    Assert.Fail(); // We should not get here!
                }
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<TaskCanceledException>();
            }

            stopTcs.TrySetResult(true);

            using( await lockQueue.GetLock(CancellationToken.None) )
            {
            }

            await task1;
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task GetLockTest3Async()
        {
            TaskLock lockQueue = new TaskLock(maxQueueSize: 100);
            var startTcs = new TaskCompletionSource<bool>();
            var stopTcs = new TaskCompletionSource<bool>();

            _ = Task.Run(async () =>
            {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    startTcs.TrySetResult(true);
                    await stopTcs.Task;
                }
            });

            try
            {
                await startTcs.Task;
                using( await lockQueue.GetLock(CancellationToken.None, TimeSpan.FromMilliseconds(1)))
                {
                    Assert.Fail(); // We should not get here!
                }
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<TaskCanceledException>();
            }

            stopTcs.TrySetResult(true);
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task GetLockTest4Async()
        {
            TaskLock lockQueue = new TaskLock(maxQueueSize: 1);

            var startTcs = new TaskCompletionSource<bool>();
            var stopTcs = new TaskCompletionSource<bool>();

            _ = Task.Run(async () =>
            {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    startTcs.TrySetResult(true);
                    await stopTcs.Task;
                }
            });

            try
            {
                await startTcs.Task;
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    Assert.Fail(); // We should not get here!
                }
            }
            catch( Exception ex )
            {
                ex.ShouldBeOfType<IndexOutOfRangeException>();
            }

            stopTcs.SetResult(true);
        }

    }
}