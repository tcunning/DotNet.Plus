using System;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class ConvertUncheckedDoubleTests
    {
        [TestMethod]
        public void ChangeTypeDoubleTest()
        {
            ConvertUnchecked.ChangeType<Double>(true).ShouldBe((double)1);
            ConvertUnchecked.ChangeType<Double>(false).ShouldBe((double)0);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>(Byte.MaxValue).ShouldBe(Byte.MaxValue);
            ConvertUnchecked.ChangeType<Double>((double)0x00).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>((double)0x01).ShouldBe((double)0x01);
            ConvertUnchecked.ChangeType<Double>(Byte.MinValue).ShouldBe(Byte.MinValue);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>(255M).ShouldBe((double)255);
            ConvertUnchecked.ChangeType<Double>(0M).ShouldBe((double)0);
            ConvertUnchecked.ChangeType<Double>(1M).ShouldBe((double)1);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>(255d).ShouldBe((double)255);
            ConvertUnchecked.ChangeType<Double>(0d).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>(1d).ShouldBe((double)0x01);
            ConvertUnchecked.ChangeType<Double>(-1d).ShouldBe((double)-1);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>((Int16)0x0000).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>((Int16)0x0001).ShouldBe((double)0x01);
            ConvertUnchecked.ChangeType<Double>((Int16)(-1)).ShouldBe((double)-1);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>((Int32)0x0000).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>((Int32)0x0001).ShouldBe((double)0x01);
            ConvertUnchecked.ChangeType<Double>((Int32)(-1)).ShouldBe((double)-1);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>((Int64)0x0000).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>((Int64)0x0001).ShouldBe((double)0x01);
            ConvertUnchecked.ChangeType<Double>((Int64)(-1)).ShouldBe((double)-1);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>((sbyte)0x00).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>((sbyte)0x01).ShouldBe((double)0x01);
            ConvertUnchecked.ChangeType<Double>((sbyte)(-1)).ShouldBe((double)-1);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>(255f).ShouldBe((double)0xFF);
            ConvertUnchecked.ChangeType<Double>(0f).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>(1f).ShouldBe((double)0x01);
            ConvertUnchecked.ChangeType<Double>(-1f).ShouldBe((double)-1);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>((UInt16)0x0000).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>((UInt16)0x0001).ShouldBe((double)0x01);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>((UInt32)0x0000).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>((UInt32)0x0001).ShouldBe((double)0x01);
            /* ----                     Double                              ---- */
            ConvertUnchecked.ChangeType<Double>((UInt64)0x0000).ShouldBe((double)0x00);
            ConvertUnchecked.ChangeType<Double>((UInt64)0x0001).ShouldBe((double)0x01);

            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<Double>(DateTime.Now));

        }

        #region enum Test
        enum TestEnum
        {
            Value1 = 10,
            Value2 = 0xFF
        }

        [TestMethod]
        public void ChangeTypeEnumTest()
        {
            ConvertUnchecked.ChangeType<double>(TestEnum.Value1).ShouldBe(10.0);
            ConvertUnchecked.ChangeType<double>(TestEnum.Value2).ShouldBe(255.0);
        }
        #endregion

    }

}