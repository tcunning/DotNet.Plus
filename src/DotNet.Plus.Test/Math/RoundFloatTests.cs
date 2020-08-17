using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Math.Tests
{
    [TestClass]
    public class RoundFloatTests
    {
        [DataTestMethod]
        [DataRow(1.4f, 1)]
        [DataRow(1.5f, 1)]
        [DataRow(1.6f, 1)]
        [DataRow(1.7f, 1)]
        [DataRow(1.8f, 1)]
        public void ValueToIntWithThresholdZero(float value, int result)
        {
            value.ToInt(Round.AlwaysRoundDownThreshold).ShouldBe(result);
            value.ToInt(0f).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.4f, 2)]
        [DataRow(1.5f, 2)]
        [DataRow(1.6f, 2)]
        [DataRow(1.7f, 2)]
        [DataRow(1.8f, 2)]
        public void ValueToIntWithThresholdOne(float value, int result)
        {
            value.ToInt(Round.AlwaysRoundUpThreshold).ShouldBe(result);
            value.ToInt(1f).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.4f, 1)]
        [DataRow(1.5f, 2)]
        [DataRow(1.6f, 2)]
        [DataRow(1.7f, 2)]
        [DataRow(1.8f, 2)]
        public void ValueToIntWithThresholdPoint5(float value, int result)
        {
            value.ToInt(0.5f).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.4f, 1)]
        [DataRow(1.5f, 1)]
        [DataRow(1.6f, 2)]
        [DataRow(1.7f, 2)]
        [DataRow(1.8f, 2)]
        public void ValueToIntWithThresholdPoint6(float value, int result)
        {
            value.ToInt(0.6f).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.4f, 1)]
        [DataRow(1.5f, 1)]
        [DataRow(1.6f, 1)]
        [DataRow(1.7f, 2)]
        [DataRow(1.8f, 2)]
        public void ValueToIntWithThresholdPoint7(float value, int result)
        {
            value.ToInt(0.7f).ShouldBe(result);
        }

        [TestMethod]
        public void ValueToIntWithThresholdFailures()
        {
            var value = 1.5f;
            Should.Throw<ArgumentOutOfRangeException>(() => value.ToInt(-0.1f));
            Should.Throw<ArgumentOutOfRangeException>(() => value.ToInt(1.1f));
        }
    }
}
