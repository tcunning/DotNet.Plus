using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Math.Tests
{
    [TestClass]
    public class RoundDecimalTests
    {
        [TestMethod]
        public void ValueToIntWithThresholdZero()
        {
            1.4M.ToInt(Round.AlwaysRoundDownThreshold).ShouldBe(1);
            1.5M.ToInt(0M).ShouldBe(1);
            1.6M.ToInt(0M).ShouldBe(1);
            1.7M.ToInt(0M).ShouldBe(1);
            1.8M.ToInt(0M).ShouldBe(1);
        }

        [TestMethod]
        public void ValueToIntWithThresholdOne()
        {
            1.4M.ToInt(Round.AlwaysRoundUpThreshold).ShouldBe(2);
            1.5M.ToInt(1M).ShouldBe(2);
            1.6M.ToInt(1M).ShouldBe(2);
            1.7M.ToInt(1M).ShouldBe(2);
            1.8M.ToInt(1M).ShouldBe(2);
        }

        [TestMethod]
        public void ValueToIntWithThresholdPoint5()
        {
            1.4M.ToInt(0.5M).ShouldBe(1);
            1.5M.ToInt(0.5M).ShouldBe(2);
            1.6M.ToInt(0.5M).ShouldBe(2);
            1.7M.ToInt(0.5M).ShouldBe(2);
            1.8M.ToInt(0.5M).ShouldBe(2);
        }

        [TestMethod]
        public void ValueToIntWithThresholdPoint6()
        {
            1.4M.ToInt(0.6M).ShouldBe(1);
            1.5M.ToInt(0.6M).ShouldBe(1);
            1.6M.ToInt(0.6M).ShouldBe(2);
            1.7M.ToInt(0.6M).ShouldBe(2);
            1.8M.ToInt(0.6M).ShouldBe(2);
        }

        [TestMethod]
        public void ValueToIntWithThresholdPoint7()
        {
            1.4M.ToInt(0.7M).ShouldBe(1);
            1.5M.ToInt(0.7M).ShouldBe(1);
            1.6M.ToInt(0.7M).ShouldBe(1);
            1.7M.ToInt(0.7M).ShouldBe(2);
            1.8M.ToInt(0.7M).ShouldBe(2);
        }

        [TestMethod]
        public void ValueToIntWithThresholdFailures()
        {
            var value = 1.5M;
            Should.Throw<ArgumentOutOfRangeException>(() => value.ToInt(-0.1M));
            Should.Throw<ArgumentOutOfRangeException>(() => value.ToInt(1.1M));
        }
    }
}
