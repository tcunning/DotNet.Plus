using System;
using System.Threading.Tasks;
using DotNet.Plus.Time;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Time
{
    [TestClass]
    public class FreeRunningTimerTests
    {
        [TestMethod]
        public async Task ChangeTypeUInt16TestAsync()
        {
            var timeStamp = FreeRunningTimer.ElapsedTime;
            await Task.Delay(10);
            var timeStamp2 = FreeRunningTimer.ElapsedTime;
            
            if( FreeRunningTimer.IsHighResolution )
                (timeStamp2 - timeStamp).TotalMilliseconds.ShouldBeGreaterThanOrEqualTo(10);
        }
    }
}
