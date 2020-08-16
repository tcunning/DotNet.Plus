using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class SetOnceValueTests
    {
        [TestMethod]
        public void SetOnceBoolTest()
        {
            var test = new SetOnceValue<bool>();
            test.IsSet.ShouldBe(false);
            test.Value.ShouldBe(default);
            test.SetOnce(true).ShouldBe(true);
            test.Value.ShouldBe(true);
            test.SetOnce(true).ShouldBe(false);
            test.SetOnce(false).ShouldBe(false);
            test.Value.ShouldBe(true);
        }

        [TestMethod]
        public void SetOnceIntTest()
        {
            var test = new SetOnceValue<int>();
            test.IsSet.ShouldBe(false);
            test.Value.ShouldBe(default);
            test.SetOnce(20).ShouldBe(true);
            test.Value.ShouldBe(20);
            test.SetOnce(20).ShouldBe(false);
            test.SetOnce(30).ShouldBe(false);
            test.Value.ShouldBe(20);
        }
    }
}