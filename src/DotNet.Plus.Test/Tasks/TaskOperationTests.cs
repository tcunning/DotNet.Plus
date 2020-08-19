using System;
using System.Threading.Tasks;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass()]
    public class TaskOperationTests
    {
        [TestMethod()]
        public void TryCatchTest()
        {
            Operation.TryCatch<bool>(() => true, false).ShouldBe(true);
            Operation.TryCatch<bool>(() => throw new Exception(), false).ShouldBe(false);
            
            TaskOperation.TryCatchAsync<bool>(() => Task.FromResult(true), false).Result.ShouldBe(true);
            TaskOperation.TryCatchAsync<bool>(() => throw new Exception(), false).Result.ShouldBe(false);

        }
    }
}