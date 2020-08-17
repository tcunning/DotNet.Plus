using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Math.Tests
{
    [TestClass]
    public class RoundSnapToDoubleTests
    {
        [TestMethod]
        public void SnapToListMiscTest()
        {
            var value = 10.0;

            foreach( SnapDirection snapDirection in Enum.GetValues(typeof(SnapDirection)) )
            {
                value.SnapTo(snapDirection, null).ShouldBe(10.0);
                value.SnapTo(snapDirection).ShouldBe(10.0);
                value.SnapTo(snapDirection, 5).ShouldBe(5.0);
                value.SnapTo(snapDirection, 5, 9).ShouldBe(9.0);
                value.SnapTo(snapDirection, 1.0, 5.0, 10.0, 15.0).ShouldBe(10.0);
                Should.Throw<ArgumentOutOfRangeException>(() => value.SnapTo(snapDirection, 1.0, 2.0, 4.0, 3.0));
            }

            Should.Throw<ArgumentOutOfRangeException>(() => value.SnapTo((SnapDirection)int.MaxValue, 1.0, 2.0, 3.0, 4.0, 20.0));
        }

        [DataTestMethod]
        [DataRow(1.0, 2.0)]
        [DataRow(2.0, 2.0)]
        [DataRow(2.1, 2.0)]
        [DataRow(3.0, 4.0)]
        [DataRow(5.0, 6.0)]
        [DataRow(11, 10.0)]
        [DataRow(15.0, 20.0)]
        [DataRow(16.0, 20.0)]
        [DataRow(20.0, 20.0)]
        [DataRow(21.0, 20.0)]
        public void SnapToListNearestRoundUpTest(double value, double result)
        {
            value.SnapTo(SnapDirection.NearestRoundUp, 2.0, 4.0, 6.0, 10.0, 20.0).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.0, 2.0)]
        [DataRow(2.0, 2.0)]
        [DataRow(2.1, 2.0)]
        [DataRow(3.0, 2.0)]
        [DataRow(5.0, 4.0)]
        [DataRow(15.0, 10.0)]
        [DataRow(16.0, 20.0)]
        [DataRow(20.0, 20.0)]
        [DataRow(21.0, 20.0)]
        public void SnapToListNearestRoundDownTest(double value, double result)
        {
            value.SnapTo(SnapDirection.NearestRoundDown, 2.0, 4.0, 6.0, 10.0, 20.0).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.0, 2)]
        [DataRow(2.0, 2)]
        [DataRow(2.1, 4)]
        [DataRow(3.0, 4)]
        [DataRow(5.0, 6)]
        [DataRow(15.0, 20)]
        [DataRow(16.0, 20)]
        [DataRow(20.0, 20)]
        [DataRow(21.0, 20)]
        public void SnapToListAlwaysRoundDownTest(double value, double result)
        {
            value.SnapTo(SnapDirection.AlwaysRoundUp, 2.0, 4.0, 6.0, 10.0, 20.0).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1.0, 2.0)]
        [DataRow(2.0, 2.0)]
        [DataRow(2.1, 2.0)]
        [DataRow(3.0, 2.0)]
        [DataRow(5.0, 4.0)]
        [DataRow(15.0, 10.0)]
        [DataRow(16.0, 10.0)]
        [DataRow(20.0, 20.0)]
        [DataRow(21.0, 20.0)]
        public void SnapToListAlwaysRoundUpTest(double value, double result)
        {
            value.SnapTo(SnapDirection.AlwaysRoundDown, 2.0, 4.0, 6.0, 10.0, 20.0).ShouldBe(result);
        }
    }
}