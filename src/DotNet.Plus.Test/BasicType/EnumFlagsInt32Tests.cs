using System;
using DotNet.Plus.BasicType;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class EnumFlagsInt32Tests
    {
        [Flags]
        enum TestFlags : UInt32
        {
            Test1 = 0x0001,
            Test2 = 0x0002,

            None = 0x0000,
            Test3 = Test1 | Test2
        }

        [TestMethod]
        public void SetFlagTest()
        {
            TestFlags.Test1.SetFlag(TestFlags.Test1).ShouldBe(TestFlags.Test1);
            TestFlags.Test1.SetFlag(TestFlags.Test2).ShouldBe(TestFlags.Test3);
            TestFlags.Test3.ClearFlag(TestFlags.Test2).ShouldBe(TestFlags.Test1);
            TestFlags.Test3.ClearFlag(TestFlags.None).ShouldBe(TestFlags.Test3);

            TestFlags.Test1.UpdateFlag(TestFlags.Test1, true).ShouldBe(TestFlags.Test1);
            TestFlags.Test1.UpdateFlag(TestFlags.Test2, true).ShouldBe(TestFlags.Test3);
            TestFlags.Test3.UpdateFlag(TestFlags.Test2, false).ShouldBe(TestFlags.Test1);
            TestFlags.Test3.UpdateFlag(TestFlags.None, false).ShouldBe(TestFlags.Test3);
        }

        [Flags]
        enum TestFlags2 : Int32
        {
            Test1 = 0x0001,
            Test2 = 0x0002,

            None = 0x0000,
            Test3 = Test1 | Test2
        }

        [TestMethod]
        public void SetFlagTest2()
        {
            TestFlags2.Test1.SetFlag(TestFlags2.Test1).ShouldBe(TestFlags2.Test1);
            TestFlags2.Test1.SetFlag(TestFlags2.Test2).ShouldBe(TestFlags2.Test3);
            TestFlags2.Test3.ClearFlag(TestFlags2.Test2).ShouldBe(TestFlags2.Test1);
            TestFlags2.Test3.ClearFlag(TestFlags2.None).ShouldBe(TestFlags2.Test3);

            TestFlags2.Test1.UpdateFlag(TestFlags2.Test1, true).ShouldBe(TestFlags2.Test1);
            TestFlags2.Test1.UpdateFlag(TestFlags2.Test2, true).ShouldBe(TestFlags2.Test3);
            TestFlags2.Test3.UpdateFlag(TestFlags2.Test2, false).ShouldBe(TestFlags2.Test1);
            TestFlags2.Test3.UpdateFlag(TestFlags2.None, false).ShouldBe(TestFlags2.Test3);
        }

    }
}