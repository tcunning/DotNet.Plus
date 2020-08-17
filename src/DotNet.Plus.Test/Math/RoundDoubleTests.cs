using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Plus.Math;
using Shouldly;

namespace DotNet.Plus.Test.Math
{
    [TestClass]
    public class RoundDoubleTests
    {
        [DataTestMethod]
        [DataRow(1.4, 1)]
        [DataRow(1.5, 1)]
        [DataRow(1.6, 1)]
        [DataRow(1.7, 1)]
        [DataRow(1.8, 1)]
        public void ValueToIntWithThresholdZero(double value, int result)
        {
            value.ToInt(Round.AlwaysRoundDownThreshold).ShouldBe(result);
            value.ToInt(0).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.4, 2)]
        [DataRow(1.5, 2)]
        [DataRow(1.6, 2)]
        [DataRow(1.7, 2)]
        [DataRow(1.8, 2)]
        public void ValueToIntWithThresholdOne(double value, int result)
        {
            value.ToInt(Round.AlwaysRoundUpThreshold).ShouldBe(result);
            value.ToInt(1).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.4, 1)]
        [DataRow(1.5, 2)]
        [DataRow(1.6, 2)]
        [DataRow(1.7, 2)]
        [DataRow(1.8, 2)]
        public void ValueToIntWithThresholdPoint5(double value, int result)  
        {
            value.ToInt(0.5).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.4, 1)]
        [DataRow(1.5, 1)]
        [DataRow(1.6, 2)]
        [DataRow(1.7, 2)]
        [DataRow(1.8, 2)]
        public void ValueToIntWithThresholdPoint6(double value, int result)
        {
            value.ToInt(0.6).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.4, 1)]
        [DataRow(1.5, 1)]
        [DataRow(1.6, 1)]
        [DataRow(1.7, 2)]
        [DataRow(1.8, 2)]
        public void ValueToIntWithThresholdPoint7(double value, int result)
        {
            value.ToInt(0.7).ShouldBe(result);
        }

        [TestMethod]
        public void ValueToIntWithThresholdFailures()
        {
            var value = 1.5;
            Should.Throw<ArgumentOutOfRangeException>(() => value.ToInt(-0.1));
            Should.Throw<ArgumentOutOfRangeException>(() => value.ToInt(1.1));
        }
    }
}
