using DotNet.Plus.Math;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace DotNet.Plus.Test.Math
{
    [TestClass]
    public class ClampIntTests
    {
        [TestMethod]
        public void SameMinMaxTests()  // Make sure we apply minimum
        {
            Clamp.Value((int)0, (int)0, (int)0).ShouldBe((int)0);

            Clamp.Value((int)1, (int)0, (int)0).ShouldBe((int)0);

            Clamp.Value((int)0, (int)1, (int)1).ShouldBe((int)1);

            Clamp.Value((int)1, (int)1, (int)1).ShouldBe((int)1);
        }

        [TestMethod]
        public void MinTests()  // Make sure we apply minimum
        {
            Clamp.Value((int)1, (int)10, (int)100).ShouldBe((int)10);
        }

        [TestMethod]
        public void MaxTests()
        {
            Clamp.Value((int)101, (int)10, (int)100).ShouldBe((int)100);
        }

        [TestMethod]
        public void InRangeTests()
        {
            Clamp.Value((int)10, (int)10, (int)100).ShouldBe((int)10);

            Clamp.Value((int)11, (int)10, (int)100).ShouldBe((int)11);
            
            Clamp.Value((int)99, (int)10, (int)100).ShouldBe((int)99);
            
            Clamp.Value((int)100, (int)10, (int)100).ShouldBe((int)100);
        }

        [TestMethod]
        public void InvalidMinMaxTests()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => Clamp.Value((int) 10, (int) 100, (int) 1));
        }

        [TestMethod]
        public void SpeedInRangeTemplateTest()
        {
            for( int index = 0; index < 2_000_000; index += 1 )
                Clamp.Value((int)50, (int)10, (int)100).ShouldBe((int)50);
        }

        [TestMethod]
        public void SpeedInRangeIntTest()
        {
            for (int index = 0; index < 2_000_000; index += 1)
                Clamp.ValueInt(50, 10, 100).ShouldBe(50);
        }

        [TestMethod]
        public void SpeedInRangeIntFastTest()
        {
            for (int index = 0; index < 2_000_000; index += 1)
                DotNet.Plus.Fast.Clamp.Value(50, 10, 100).ShouldBe(50);
        }
    }
}
