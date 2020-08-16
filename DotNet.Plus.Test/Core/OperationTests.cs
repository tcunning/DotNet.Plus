using System;
using System.Threading.Tasks;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass()]
    public class OperationTests
    {
        [TestMethod()]
        public void TryCatchTest()
        {
            Operation.TryCatch<bool>(() => true, false).ShouldBe(true);
            Operation.TryCatch<bool>(() => throw new Exception(), false).ShouldBe(false);
            
            Operation.TryCatchAsync<bool>(() => Task.FromResult(true), false).Result.ShouldBe(true);
            Operation.TryCatchAsync<bool>(() => throw new Exception(), false).Result.ShouldBe(false);

        }
    }
}