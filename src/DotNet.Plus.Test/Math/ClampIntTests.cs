using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Plus.Math;
using Shouldly;

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

    }
}
