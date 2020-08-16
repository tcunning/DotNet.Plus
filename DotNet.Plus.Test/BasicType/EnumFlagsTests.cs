using System;
using System.Reflection;
using DotNet.Plus.Core;
using DotNet.Plus.BasicType;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class EnumFlagsTests
    {
        [Flags]
        enum TestFlags
        {
            Test1 = 0x0001,
            Test2 = 0x0002,

            None = 0x0000,
            Test3 = Test1 | Test2
        }

        enum Test
        {
            Test1 = 0x0001,
        }

        [TestMethod]
        public void SetFlagTest()
        {
            TestFlags.Test1.SetFlag(TestFlags.Test1).ShouldBe(TestFlags.Test1);
            TestFlags.Test1.SetFlag(TestFlags.Test2).ShouldBe(TestFlags.Test3);
            TestFlags.Test1.ClearFlag(TestFlags.Test1).ShouldBe(TestFlags.None);
            TestFlags.Test3.ClearFlag(TestFlags.Test2).ShouldBe(TestFlags.Test1);
            TestFlags.Test3.ClearFlag(TestFlags.None).ShouldBe(TestFlags.Test3);

            TestFlags.Test1.UpdateFlag(TestFlags.Test1, true).ShouldBe(TestFlags.Test1);
            TestFlags.Test1.UpdateFlag(TestFlags.Test2, true).ShouldBe(TestFlags.Test3);
            TestFlags.Test3.UpdateFlag(TestFlags.Test2, false).ShouldBe(TestFlags.Test1);
            TestFlags.Test3.UpdateFlag(TestFlags.None, false).ShouldBe(TestFlags.Test3);

            Should.Throw<NotFlagsEnumException>(() => Test.Test1.UpdateFlag(Test.Test1, true));
            Should.Throw<NotFlagsEnumException>(() => Test.Test1.UpdateFlag(Test.Test1, false));

            typeof(EnumFlags).SetProperty(null, "InjectTypeCode", TypeCode.Empty, BindingFlags.Static | BindingFlags.NonPublic);
            Should.Throw<ArgumentException>(() => TestFlags.Test1.SetFlag(TestFlags.Test2));
            Should.Throw<ArgumentException>(() => TestFlags.Test1.ClearFlag(TestFlags.Test1));
            typeof(EnumFlags).SetProperty<TypeCode?>(null, "InjectTypeCode", null, BindingFlags.Static | BindingFlags.NonPublic);

        }
    }
}