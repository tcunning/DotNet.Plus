using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Threading.Tasks;
using Shouldly;

namespace DotNet.Plus.Tasks.Tests
{
    [TestClass]
    public class TaskDelayTests
    {
        [TestMethod]
        public void DelayTestAsync()
        {
            using var cts = new CancellationTokenSource();
            var result = TaskDelay.Delay(TimeSpan.FromMilliseconds(5), cts.Token);
            cts.Cancel();
            result.Exception.ShouldNotBeOfType<TaskCanceledException>();
        }

        [TestMethod]
        public void TryDelayTestAsync()
        {
            using var cts = new CancellationTokenSource();
            var result = TaskDelay.TryDelay(5, cts.Token);
            cts.Cancel();
            result.Result.ShouldBe(false);
        }

        [TestMethod]
        public void TryDelayTimeSpanTestAsync()
        {
            using var cts = new CancellationTokenSource();
            var result = TaskDelay.TryDelay(TimeSpan.FromMilliseconds(5), cts.Token);
            cts.Cancel();
            result.Result.ShouldBe(false);
        }

    }
}