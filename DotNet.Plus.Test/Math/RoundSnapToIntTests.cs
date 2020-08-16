using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Shouldly;

namespace DotNet.Plus.Math.Tests
{
    [TestClass]
    public class RoundSnapToIntTests
    {
        [TestMethod]
        public void SnapToListMiscTest()
        {
            var value = 10;

            foreach( SnapDirection snapDirection in Enum.GetValues(typeof(SnapDirection)) )
            {
                value.SnapTo(snapDirection, null).ShouldBe(10);
                value.SnapTo(snapDirection).ShouldBe(10);
                value.SnapTo(snapDirection, 5).ShouldBe(5);
                value.SnapTo(snapDirection, 5, 9).ShouldBe(9);
                value.SnapTo(snapDirection, 1, 5, 10, 15).ShouldBe(10);
                Should.Throw<ArgumentOutOfRangeException>(() => value.SnapTo(snapDirection, 1, 2, 4, 3));
            }

            Should.Throw<ArgumentOutOfRangeException>(() => value.SnapTo((SnapDirection)int.MaxValue, 1, 2, 3, 4, 20));
        }

        [DataTestMethod]
        [DataRow(1, 2)]
        [DataRow(2, 2)]
        [DataRow(3, 4)]
        [DataRow(5, 6)]
        [DataRow(11, 10)]
        [DataRow(15, 20)]
        [DataRow(16, 20)]
        [DataRow(20, 20)]
        [DataRow(21, 20)]
        public void SnapToListNearestRoundUpTest(int value, int result)
        {
            value.SnapTo(SnapDirection.NearestRoundUp, 2, 4, 6, 10, 20).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1, 2)]
        [DataRow(2, 2)]
        [DataRow(3, 2)]
        [DataRow(5, 4)]
        [DataRow(15, 10)]
        [DataRow(16, 20)]
        [DataRow(20, 20)]
        [DataRow(21, 20)]
        public void SnapToListNearestRoundDownTest(int value, int result)
        {
            value.SnapTo(SnapDirection.NearestRoundDown, 2, 4, 6, 10, 20).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1, 2)]
        [DataRow(2, 2)]
        [DataRow(3, 4)]
        [DataRow(5, 6)]
        [DataRow(15, 20)]
        [DataRow(16, 20)]
        [DataRow(20, 20)]
        [DataRow(21, 20)]
        public void SnapToListAlwaysRoundDownTest(int value, int result)
        {
            value.SnapTo(SnapDirection.AlwaysRoundUp, 2, 4, 6, 10, 20).ShouldBe(result);
        }

        [DataTestMethod]
        [DataRow(1, 2)]
        [DataRow(2, 2)]
        [DataRow(3, 2)]
        [DataRow(5, 4)]
        [DataRow(15, 10)]
        [DataRow(16, 10)]
        [DataRow(20, 20)]
        [DataRow(21, 20)]
        public void SnapToListAlwaysRoundUpTest(int value, int result)
        {
            value.SnapTo(SnapDirection.AlwaysRoundDown, 2, 4, 6, 10, 20).ShouldBe(result);
        }
    }
}