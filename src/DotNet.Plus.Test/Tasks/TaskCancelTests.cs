using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;

namespace DotNet.Plus.Tasks.Tests
{
    [TestClass]
    public class TaskCancelTests
    {
        [TestMethod]
        [Timeout(100)]
        public async Task CancelWhenWithResultImmediateTestAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            using var cts = new CancellationTokenSource();
            tcs.SetResult(10);

            var result = await tcs.CancelWhen(cts.Token);
            result.ShouldBe(10);
        }


        [TestMethod]
        [Timeout(100)]
        public void CancelWhenCanceledTestAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            using var cts = new CancellationTokenSource();
            cts.Cancel();
            cts.IsCancellationRequested.ShouldBe(true);

            Should.Throw<TaskCanceledException>(async () => await tcs.CancelWhen(cts.Token));
        }

        [TestMethod]
        [Timeout(100)]
        public void CancelWhenTimeoutTestAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            Should.Throw<TimeoutException>(async () => await tcs.CancelWhen(2));
        }

        [TestMethod]
        [Timeout(100)]
        public async Task CancelWhenWithResultTestAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            using var cts = new CancellationTokenSource();

            _ = Task.Run(async () => {
                await Task.Delay(1, CancellationToken.None);
                tcs.SetResult(10);

            });

            var result = await tcs.CancelWhen(cts.Token);
            result.ShouldBe(10);
        }

        [TestMethod]
        [Timeout(100)]
        public void CancelWhenExceptionTestAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            using var cts = new CancellationTokenSource();

            _ = Task.Run(async () => {
                await Task.Delay(2, CancellationToken.None);
                tcs.SetException(new ArgumentException());
            });

            Should.Throw<ArgumentException>(async () => await tcs.CancelWhen(cts.Token));
        }

        [TestMethod]
        [Timeout(100)]
        public void CancelWhenTaskCanceledTestAsync()
        {
            var tcs = new TaskCompletionSource<int>();
            using var cts = new CancellationTokenSource();

            _ = Task.Run(async () => {
                await Task.Delay(2, CancellationToken.None);
                tcs.SetCanceled();
            });

            Should.Throw<TaskCanceledException>(async () => await tcs.CancelWhen(cts.Token));
        }


    }
}