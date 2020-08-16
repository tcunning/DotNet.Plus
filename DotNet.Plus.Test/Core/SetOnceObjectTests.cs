using System;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass()]
    public class SetOnceObjectTests
    {
        public class Test
        {
        }

        [TestMethod]
        public void SetOnceObjectTest()
        {
            var testObj = new Test();
            var testObj2 = new Test();

            var test = new SetOnceObject<Test>();
            test.IsSet.ShouldBe(false);
            test.Value.ShouldBe(default);
            test.SetOnce(testObj).ShouldBe(true);
            test.Value.ShouldBeSameAs(testObj);
            test.SetOnce(testObj).ShouldBe(false);
            test.SetOnce(testObj2).ShouldBe(false);
            test.Value.ShouldBeSameAs(testObj);
            test.SetOnce(null!);
        }

        [TestMethod]
        public void SetOnceNullTest()
        {
            var test = new SetOnceObject<Test>();
            Should.Throw<ArgumentNullException>(() => test.SetOnce(null!));
        }
    }
}