using DotNet.Plus.Tasks;
using DotNet.Plus.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotNet.Plus.Test.Time
{
    [TestClass]
    public class WatchdogTests
    {
        [TestMethod]
        public void WatchdogInitialStateTest()
        {
            var wd = new Watchdog(petTimeout: TimeSpan.FromMilliseconds(100), autoStartOnFirstPet: false);
            wd.IsTriggered.ShouldBe(false);
            wd.IsCanceled.ShouldBe(false);
            wd.IsDisposed.ShouldBe(false);
            wd.IsMonitorStarted.ShouldBe(false);
        }
        
        [TestMethod]
        [Timeout(1000)]
        public async Task WatchdogFailureTestAsync()
        {
            var wd = new Watchdog(petTimeout: TimeSpan.FromMilliseconds(100), autoStartOnFirstPet: false);
            Should.Throw<WatchdogNotStartedException>(() => wd.Cancel());
            Should.Throw<WatchdogNotStartedException>(() => wd.Pet(autoReset: false));

            wd.Monitor();
            wd.IsTriggered.ShouldBe(false);
            wd.IsCanceled.ShouldBe(false);
            wd.IsDisposed.ShouldBe(false);
            wd.IsMonitorStarted.ShouldBe(true);
            Should.Throw<WatchdogAlreadyStartedException>(() => wd.Monitor());

            await 200.TryDelay(CancellationToken.None);
            wd.IsTriggered.ShouldBe(true);
            wd.IsCanceled.ShouldBe(false);
            wd.IsDisposed.ShouldBe(false);
            wd.IsMonitorStarted.ShouldBe(true);

            Should.Throw<WatchdogTriggeredException>(() => wd.Monitor());
            Should.Throw<WatchdogTriggeredException>(() => wd.Cancel());

            wd.AsTask().Result.ShouldBe(true);  // Triggered

            wd.TryDispose();
            wd.IsTriggered.ShouldBe(true);
            wd.IsCanceled.ShouldBe(false);
            wd.IsDisposed.ShouldBe(true);
            wd.IsMonitorStarted.ShouldBe(true);
            Should.Throw<WatchdogDisposedException>(() => wd.Cancel());
            Should.Throw<WatchdogDisposedException>(() => wd.Pet(autoReset: false));
            Should.Throw<WatchdogDisposedException>(() => wd.Monitor());
            Should.Throw<WatchdogDisposedException>(() => throw wd.AsTask().Exception!);
        }

        [TestMethod]
        public void WatchdogDisposeNotStartedTest()
        {
            var wd = new Watchdog(petTimeout: TimeSpan.FromMilliseconds(100), autoStartOnFirstPet: false);
            wd.Dispose();
            wd.IsTriggered.ShouldBe(false);
            wd.IsCanceled.ShouldBe(false);
            wd.IsDisposed.ShouldBe(true);
            wd.IsMonitorStarted.ShouldBe(false);
        }

        [TestMethod]
        public void WatchdogDisposeStartedTest()
        {
            var wd = new Watchdog(petTimeout: TimeSpan.FromMilliseconds(100), autoStartOnFirstPet: false);
            var wdTask = wd.AsTask();
            wd.Monitor();
            wd.Dispose();
            wd.IsTriggered.ShouldBe(false);
            wd.IsCanceled.ShouldBe(true);
            wd.IsDisposed.ShouldBe(true);
            wd.IsMonitorStarted.ShouldBe(true);
            Should.Throw<WatchdogDisposedException>(() => throw wdTask.Exception!);
            Should.Throw<WatchdogDisposedException>(() => throw wd.AsTask().Exception!);
        }
        
        [TestMethod]
        public void WatchdogCancelBeforeTriggeredTest()
        {
            var wd = new Watchdog(petTimeout: TimeSpan.FromMilliseconds(100), autoStartOnFirstPet: false);
            var wdTask = wd.AsTask();
            wd.Monitor();
            wd.Cancel();
            wd.IsTriggered.ShouldBe(false);
            wd.IsCanceled.ShouldBe(true);
            wd.IsDisposed.ShouldBe(false);
            wd.IsMonitorStarted.ShouldBe(true);
            Should.Throw<WatchdogCanceledException>(() => throw wdTask.Exception!);
            Should.Throw<WatchdogCanceledException>(() => throw wd.AsTask().Exception!);
        }
        
        [TestMethod]
        public void WatchdogCancelAsTaskTest()
        {
            var wd = new Watchdog(petTimeout: TimeSpan.FromMilliseconds(100), autoStartOnFirstPet: false);
            wd.Monitor();
            wd.Cancel();
            Should.Throw<WatchdogCanceledException>(() => throw wd.AsTask().Exception!);
        }

        [TestMethod]
        [Timeout(1000)]
        public async Task WatchdogPetTestAsync()
        {
            var wd = new Watchdog(petTimeout: TimeSpan.FromMilliseconds(100), autoStartOnFirstPet: true);
            var wdTask = wd.AsTask();

            wd.Pet(autoReset: false);
            wd.IsTriggered.ShouldBe(false);
            wd.IsCanceled.ShouldBe(false);
            wd.IsDisposed.ShouldBe(false);
            wd.IsMonitorStarted.ShouldBe(true);
            wd.AsTask().IsCompleted.ShouldBe(false);

            await 20.TryDelay(CancellationToken.None);
            var ms = wd.Pet(autoReset: false);
            wd.IsTriggered.ShouldBe(false);
            ms.ShouldBeInRange(TimeSpan.FromMilliseconds(15), TimeSpan.FromMilliseconds(50));

            await 20.TryDelay(CancellationToken.None);
            ms = wd.Pet(autoReset: false);
            wd.IsTriggered.ShouldBe(false);
            ms.ShouldBeInRange(TimeSpan.FromMilliseconds(15), TimeSpan.FromMilliseconds(50));

            await 20.TryDelay(CancellationToken.None);
            ms = wd.Pet(autoReset: false);
            wd.IsTriggered.ShouldBe(false);
            ms.ShouldBeInRange(TimeSpan.FromMilliseconds(15), TimeSpan.FromMilliseconds(50));

            await 20.TryDelay(CancellationToken.None);
            ms = wd.Pet(autoReset: false);
            wd.IsTriggered.ShouldBe(false);
            ms.ShouldBeInRange(TimeSpan.FromMilliseconds(15), TimeSpan.FromMilliseconds(50));

            await 20.TryDelay(CancellationToken.None);
            ms = wd.Pet(autoReset: false);
            wd.IsTriggered.ShouldBe(false);
            ms.ShouldBeInRange(TimeSpan.FromMilliseconds(15), TimeSpan.FromMilliseconds(50));

            await 20.TryDelay(CancellationToken.None);
            ms = wd.Pet(autoReset: false);
            wd.IsTriggered.ShouldBe(false);
            ms.ShouldBeInRange(TimeSpan.FromMilliseconds(15), TimeSpan.FromMilliseconds(50));

            await 20.TryDelay(CancellationToken.None);
            ms = wd.Pet(autoReset: false);
            wd.IsTriggered.ShouldBe(false);
            ms.ShouldBeInRange(TimeSpan.FromMilliseconds(15), TimeSpan.FromMilliseconds(50));

            await 110.TryDelay(CancellationToken.None);
            wd.IsTriggered.ShouldBe(true);
            wd.IsCanceled.ShouldBe(false);
            wd.IsDisposed.ShouldBe(false);
            wd.IsMonitorStarted.ShouldBe(true);
            wd.AsTask().IsCompleted.ShouldBe(true);
            Should.Throw<WatchdogTriggeredException>(() => wd.Pet(autoReset: false));

            ms = wd.Pet(autoReset: true);
            ms.ShouldBeInRange(TimeSpan.FromMilliseconds(0), TimeSpan.FromMilliseconds(10));
            wd.IsTriggered.ShouldBe(false);



        }


    }
}
