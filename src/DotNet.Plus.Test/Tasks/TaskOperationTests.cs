using System;
using System.Threading.Tasks;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class TaskOperationTests
    {
        [TestMethod]
        public void TryCatchTest()
        {
            Operation.TryCatch<bool>(() => true, false).ShouldBe(true);
            Operation.TryCatch<bool>(() => throw new Exception(), false).ShouldBe(false);
            
            Task.FromResult(true).TryCatchAsync(failureValue: false).Result.ShouldBe(true);
            Task.FromException<bool>(new Exception()).TryCatchAsync(failureValue: false).Result.ShouldBe(false);
        }

        [TestMethod]
        public void TryCatchTaskWithSuccessResultTest()
        {
            Task.CompletedTask.TryCatchAsync<bool>(true).Result.ShouldBe(true);
            Task.FromException(new Exception()).TryCatchAsync(true).Result.ShouldBe(false);
        }

        [TestMethod]
        public void TryCatchTaskNoResultTest()
        {
            _ = Task.CompletedTask.TryCatchAsync();
            _ = Task.FromException(new Exception()).TryCatchAsync();
        }

    }
}