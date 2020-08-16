using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Plus.Math;
using Shouldly;

namespace DotNet.Plus.Test.Math
{
    [TestClass]
    public class ClampUIntTests
    {
        [TestMethod]
        public void SameMinMaxTests()  // Make sure we apply minimum
        {
            Clamp.Value((uint)0, (uint)0, (uint)0).ShouldBe((uint)0);

            Clamp.Value((uint)1, (uint)0, (uint)0).ShouldBe((uint)0);

            Clamp.Value((uint)0, (uint)1, (uint)1).ShouldBe((uint)1);

            Clamp.Value((uint)1, (uint)1, (uint)1).ShouldBe((uint)1);
        }

        [TestMethod]
        public void MinTests()  // Make sure we apply minimum
        {
            Clamp.Value((uint)1, (uint)10, (uint)100).ShouldBe((uint)10);
        }

        [TestMethod]
        public void MaxTests()
        {
            Clamp.Value((uint)101, (uint)10, (uint)100).ShouldBe((uint)100);
        }

        [TestMethod]
        public void InRangeTests()
        {
            Clamp.Value((uint)10, (uint)10, (uint)100).ShouldBe((uint)10);

            Clamp.Value((uint)11, (uint)10, (uint)100).ShouldBe((uint)11);
            
            Clamp.Value((uint)99, (uint)10, (uint)100).ShouldBe((uint)99);
            
            Clamp.Value((uint)100, (uint)10, (uint)100).ShouldBe((uint)100);
        }

        [TestMethod]
        public void InvalidMinMaxTests()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => Clamp.Value((uint) 10, (uint) 100, (uint) 1));
        }

    }
}
