using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using DotNet.Plus.Pattern;
using Shouldly;

namespace DotNet.Plus.Core.Tests
{
    [TestClass]
    public class CancellationTokenSourceExTests
    {
        [TestMethod]
        public void TryCancelTest()
        {
            var cts = new CancellationTokenSource();
            
            cts.IsCancellationRequested.ShouldBe(false);
            
            cts.TryCancel();
            cts.IsCancellationRequested.ShouldBe(true);

            cts.TryCancel();
            cts.IsCancellationRequested.ShouldBe(true);

            cts.TryDispose();
        }

        [TestMethod]
        public void TryCancelAndDisposeTest()
        {
            var cts = new CancellationTokenSource();

            cts.IsCancellationRequested.ShouldBe(false);

            cts.TryCancelAndDispose();
            cts.IsCancellationRequested.ShouldBe(true);

            cts.TryCancelAndDispose();
            cts.IsCancellationRequested.ShouldBe(true);

            cts.TryDispose();
        }
    }
}