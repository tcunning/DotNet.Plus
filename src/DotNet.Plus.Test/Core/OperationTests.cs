using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection.Metadata.Ecma335;
using DotNet.Plus.Core;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class OperationTests
    {
        [TestMethod]
        public void TryCatchTest()
        {
            Operation.TryCatch(() => { });
            Operation.TryCatch(() => throw new Exception());

            Operation.TryCatch<int>(() => { }, successValue: 10).ShouldBe(10);
            Operation.TryCatch<int>(() => throw new Exception(), successValue: 10).ShouldBe(0);
        }
    }
}