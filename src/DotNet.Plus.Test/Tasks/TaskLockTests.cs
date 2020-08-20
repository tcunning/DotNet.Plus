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

            var continueTcs = new TaskCompletionSource<bool>();

            var task1 = Task.Run(async () => {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    continueTcs.TrySetResult(true);
                    await TaskDelay.TryDelay(10, CancellationToken.None);
                }
            });

            await continueTcs.Task;

            try
            {
                using var preCts = new CancellationTokenSource();
                preCts.Cancel();
                using (await lockQueue.GetLock(preCts.Token))
                {
                    Assert.Fail(); // We should not get here!
                }
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<TaskCanceledException>();
            }

            using( await lockQueue.GetLock(CancellationToken.None) )
            {
                await TaskDelay.TryDelay(2, CancellationToken.None);
            }

            await task1;
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task GetLockTest3Async()
        {
            TaskLock lockQueue = new TaskLock(maxQueueSize: 100);
            var continueTcs = new TaskCompletionSource<bool>();

            var task1 = Task.Run(async () =>
            {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    continueTcs.TrySetResult(true);
                    await TaskDelay.TryDelay(20, CancellationToken.None);
                }
            });

            await continueTcs.Task;

            try
            {
                using (await lockQueue.GetLock(CancellationToken.None, TimeSpan.FromMilliseconds(1)))
                {
                    Assert.Fail(); // We should not get here!
                }
            }
            catch (Exception ex)
            {
                ex.ShouldBeOfType<TaskCanceledException>();
            }

            await task1;
        }

        [TestMethod]
        [Timeout(5000)]
        public async Task GetLockTest4Async()
        {
            TaskLock lockQueue = new TaskLock(maxQueueSize: 1);

            var continueTcs = new TaskCompletionSource<bool>();

            var task1 = Task.Run(async () =>
            {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    continueTcs.TrySetResult(true);
                    await TaskDelay.TryDelay(80, CancellationToken.None);
                }
            });

            await continueTcs.Task;

            try
            {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    Assert.Fail(); // We should not get here!
                }
            }
            catch( Exception ex )
            {
                ex.ShouldBeOfType<IndexOutOfRangeException>();
            }

            await task1;
        }

    }
}