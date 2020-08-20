using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNet.Plus.Core;

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
        }
    }
}