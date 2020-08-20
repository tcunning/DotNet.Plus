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
        [Timeout(100)]
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
        [Timeout(100)]
        public async Task GetLockTest2Async()
        {
            TaskLock lockQueue = new TaskLock(maxQueueSize: 100);

            var task1 = Task.Run(async () => {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    await TaskDelay.TryDelay(2, CancellationToken.None);
                }
            });

            using var preCts = new CancellationTokenSource();
            preCts.Cancel();
            using( await lockQueue.GetLock(preCts.Token) )
            {
            }

            using( await lockQueue.GetLock(CancellationToken.None) )
            {
                await TaskDelay.TryDelay(2, CancellationToken.None);
            }

            await task1;
        }

        [TestMethod]
        [Timeout(100)]
        public async Task GetLockTest3Async()
        {
            TaskLock lockQueue = new TaskLock(maxQueueSize: 100);

            var task1 = Task.Run(async () =>
            {
                using( await lockQueue.GetLock(CancellationToken.None) )
                {
                    await TaskDelay.TryDelay(20, CancellationToken.None);
                }
            });

            await TaskDelay.TryDelay(10, CancellationToken.None);

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

    }
}