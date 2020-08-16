using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static System.Math;
using DotNet.Plus.Math;
using Shouldly;

namespace DotNet.Plus.Test.Math
{
    [TestClass]
    public class CircleTests
    {
        [TestMethod]
        public void DegreeToRadianTests()  // Make sure we apply minimum
        {
            Circle.DegreeToRadian(0).ShouldBe(0);

            Circle.DegreeToRadian(45).ShouldBe(PI / 4);

            Circle.DegreeToRadian(90).ShouldBe(PI / 2);

            Circle.DegreeToRadian(180).ShouldBe(PI);

            Circle.DegreeToRadian(360).ShouldBe(PI * 2);

            Circle.DegreeToRadian(360 * 2).ShouldBe(PI * 4);

            Circle.DegreeToRadian(-90).ShouldBe(PI / 2 * -1);
        }

        [TestMethod]
        public void RadianToDegreeTests()  // Make sure we apply minimum
        {
            Circle.RadianToDegree(0).ShouldBe(0);

            Circle.RadianToDegree(PI / 4).ShouldBe(45);

            Circle.RadianToDegree(PI / 2).ShouldBe(90);

            Circle.RadianToDegree(PI).ShouldBe(180);

            Circle.RadianToDegree(PI * 2).ShouldBe(360);

            Circle.RadianToDegree(PI * 4).ShouldBe(360 * 2);

            Circle.RadianToDegree(PI / 2 * -1).ShouldBe(-90);
        }
    }
}
