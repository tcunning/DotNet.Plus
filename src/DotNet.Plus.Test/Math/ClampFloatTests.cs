using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Plus.Math;
using Shouldly;

namespace DotNet.Plus.Test.Math
{
    [TestClass]
    public class ClampFloatTests
    {
        [TestMethod]
        public void SameMinMaxTests()  // Make sure we apply minimum
        {
            Clamp.Value((float)0, (float)0, (float)0).ShouldBe((float)0);

            Clamp.Value((float)1, (float)0, (float)0).ShouldBe((float)0);

            Clamp.Value((float)0, (float)1, (float)1).ShouldBe((float)1);

            Clamp.Value((float)1, (float)1, (float)1).ShouldBe((float)1);
        }

        [TestMethod]
        public void MinTests()  // Make sure we apply minimum
        {
            Clamp.Value((float)1, (float)10.1, (float)100).ShouldBe((float)10.1);
        }

        [TestMethod]
        public void MaxTests()
        {
            Clamp.Value((float)101, (float)10, (float)100).ShouldBe((float)100);
        }

        [TestMethod]
        public void InRangeTests()
        {
            Clamp.Value((float)10.5, (float)10, (float)100).ShouldBe((float)10.5);

            Clamp.Value((float)11.1, (float)10, (float)100).ShouldBe((float)11.1);
            
            Clamp.Value((float)99, (float)10, (float)100).ShouldBe((float)99);
            
            Clamp.Value((float)100, (float)10, (float)100).ShouldBe((float)100);
        }

        [TestMethod]
        public void InvalidMinMaxTests()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => Clamp.Value((float) 10, (float) 100, (float) 1));
        }

    }
}
