using System;
using DotNet.Plus.BasicType;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class EnumDebugTests
    {
        enum TestFlags
        {
            Test1 = 0x0001,
            Test2 = 0x0002,
            Test4 = 0x0004,
            
            Test3 = Test1 | Test2
        }

        [TestMethod]
        public void DebugDumpAsFlagsTest()
        {
            1U.DebugDumpAsFlags<TestFlags>().ShouldBe("Test1, Test3");
            2U.DebugDumpAsFlags<TestFlags>().ShouldBe("Test2, Test3");
            3U.DebugDumpAsFlags<TestFlags>().ShouldBe("Test1, Test2, Test3");
            4U.DebugDumpAsFlags<TestFlags>().ShouldBe("Test4");
            8U.DebugDumpAsFlags<TestFlags>().ShouldBe("none");

            TestFlags.Test2.DebugDumpAsFlags<TestFlags>().ShouldBe("Test2, Test3");

            1.DebugDumpAsFlags<TestFlags>().ShouldBe("Test1, Test3");
            2.DebugDumpAsFlags<TestFlags>().ShouldBe("Test2, Test3");
            3.DebugDumpAsFlags<TestFlags>().ShouldBe("Test1, Test2, Test3");
            4.DebugDumpAsFlags<TestFlags>().ShouldBe("Test4");
            8.DebugDumpAsFlags<TestFlags>().ShouldBe("none");
            (-1).DebugDumpAsFlags<TestFlags>().ShouldBe("Test1, Test2, Test3, Test4");

        }
    }
}