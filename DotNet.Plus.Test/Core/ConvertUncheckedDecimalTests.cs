using System;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class ConvertUncheckedDecimalTests
    {
        [TestMethod]
        public void ChangeTypeDecimalTest()
        {
            ConvertUnchecked.ChangeType<Decimal>(true).ShouldBe((decimal)1);
            ConvertUnchecked.ChangeType<Decimal>(false).ShouldBe((decimal)0);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>(Byte.MaxValue).ShouldBe((decimal)255);
            ConvertUnchecked.ChangeType<Decimal>((decimal)0x00).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>((decimal)0x01).ShouldBe((decimal)0x01);
            ConvertUnchecked.ChangeType<Decimal>(Byte.MinValue).ShouldBe((decimal)0x00);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>(255M).ShouldBe((decimal)255);
            ConvertUnchecked.ChangeType<Decimal>(0M).ShouldBe((decimal)0);
            ConvertUnchecked.ChangeType<Decimal>(1M).ShouldBe((decimal)1);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>(255d).ShouldBe((decimal)255);
            ConvertUnchecked.ChangeType<Decimal>(0d).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>(1d).ShouldBe((decimal)0x01);
            ConvertUnchecked.ChangeType<Decimal>(-1d).ShouldBe((decimal)-1);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>((Int16)0x0000).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>((Int16)0x0001).ShouldBe((decimal)0x01);
            ConvertUnchecked.ChangeType<Decimal>((Int16)(-1)).ShouldBe((decimal)-1);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>((Int32)0x0000).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>((Int32)0x0001).ShouldBe((decimal)0x01);
            ConvertUnchecked.ChangeType<Decimal>((Int32)(-1)).ShouldBe((decimal)-1);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>((Int64)0x0000).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>((Int64)0x0001).ShouldBe((decimal)0x01);
            ConvertUnchecked.ChangeType<Decimal>((Int64)(-1)).ShouldBe((decimal)-1);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>((sbyte)0x00).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>((sbyte)0x01).ShouldBe((decimal)0x01);
            ConvertUnchecked.ChangeType<Decimal>((sbyte)(-1)).ShouldBe((decimal)-1);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>(255f).ShouldBe((decimal)0xFF);
            ConvertUnchecked.ChangeType<Decimal>(0f).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>(1f).ShouldBe((decimal)0x01);
            ConvertUnchecked.ChangeType<Decimal>(-1f).ShouldBe((decimal)-1);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>((UInt16)0x0000).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>((UInt16)0x0001).ShouldBe((decimal)0x01);
            //ConvertUnchecked.ChangeType<Decimal>(unchecked((UInt16)(-1))).ShouldBe((decimal)0xFF);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>((UInt32)0x0000).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>((UInt32)0x0001).ShouldBe((decimal)0x01);
            //ConvertUnchecked.ChangeType<Decimal>(unchecked((UInt32)(-1))).ShouldBe((decimal)0xFF);
            ConvertUnchecked.ChangeType<Decimal>(UInt32.MinValue).ShouldBe((decimal)0x00);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Decimal>((UInt64)0x0000).ShouldBe((decimal)0x00);
            ConvertUnchecked.ChangeType<Decimal>((UInt64)0x0001).ShouldBe((decimal)0x01);
            //ConvertUnchecked.ChangeType<Decimal>(unchecked((UInt64)(-1))).ShouldBe((decimal)0xFF);

            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<Decimal>(DateTime.Now));

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
            ConvertUnchecked.ChangeType<decimal>(TestEnum.Value1).ShouldBe((decimal)10);
            ConvertUnchecked.ChangeType<decimal>(TestEnum.Value2).ShouldBe((decimal)0xFF);
        }
        #endregion
    }

}