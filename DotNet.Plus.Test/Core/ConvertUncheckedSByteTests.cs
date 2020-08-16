using System;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class ConvertUncheckedSByteTests
    {
        [TestMethod]
        public void ChangeTypeSByteTest()
        {
            ConvertUnchecked.ChangeType<SByte>(true).ShouldBe((sbyte)1);
            ConvertUnchecked.ChangeType<SByte>(false).ShouldBe((sbyte)0);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>((byte)0x00).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((byte)0x01).ShouldBe((sbyte)0x01);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(0M).ShouldBe((sbyte)0);
            ConvertUnchecked.ChangeType<SByte>(1M).ShouldBe((sbyte)1);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(double.MaxValue).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>(0d).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>(1d).ShouldBe((sbyte)0x01);
            ConvertUnchecked.ChangeType<SByte>(-1d).ShouldBe((sbyte)-1);
            ConvertUnchecked.ChangeType<SByte>(double.MinValue).ShouldBe((sbyte)0x00);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(unchecked((Int16)0x8000)).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((Int16)0x0000).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((Int16)0x0001).ShouldBe((sbyte)0x01);
            ConvertUnchecked.ChangeType<SByte>((Int16)(-1)).ShouldBe((sbyte)-1);
            ConvertUnchecked.ChangeType<SByte>(Int16.MinValue).ShouldBe((sbyte)0x00);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(unchecked((Int32)0x8000)).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((Int32)0x0000).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((Int32)0x0001).ShouldBe((sbyte)0x01);
            ConvertUnchecked.ChangeType<SByte>((Int32)(-1)).ShouldBe((sbyte)-1);
            ConvertUnchecked.ChangeType<SByte>(Int32.MinValue).ShouldBe((sbyte)0x00);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(unchecked((Int64)0x8000)).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((Int64)0x0000).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((Int64)0x0001).ShouldBe((sbyte)0x01);
            ConvertUnchecked.ChangeType<SByte>((Int64)(-1)).ShouldBe((sbyte)-1);
            ConvertUnchecked.ChangeType<SByte>(Int64.MinValue).ShouldBe((sbyte)0x00);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(SByte.MaxValue).ShouldBe((sbyte)SByte.MaxValue);
            ConvertUnchecked.ChangeType<SByte>((sbyte)0x00).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((sbyte)0x01).ShouldBe((sbyte)0x01);
            ConvertUnchecked.ChangeType<SByte>((sbyte)(-1)).ShouldBe((sbyte)-1);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(float.MaxValue).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>(0f).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>(1f).ShouldBe((sbyte)0x01);
            ConvertUnchecked.ChangeType<SByte>(-1f).ShouldBe((sbyte)-1);
            ConvertUnchecked.ChangeType<SByte>(float.MinValue).ShouldBe((sbyte)0x00);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(unchecked((UInt16)0x8000)).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((UInt16)0x0000).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((UInt16)0x0001).ShouldBe((sbyte)0x01);
            ConvertUnchecked.ChangeType<SByte>(unchecked((UInt16)(-1))).ShouldBe((sbyte)-1);
            ConvertUnchecked.ChangeType<SByte>(UInt16.MinValue).ShouldBe((sbyte)0x00);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(unchecked((UInt32)0x8000)).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((UInt32)0x0000).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((UInt32)0x0001).ShouldBe((sbyte)0x01);
            ConvertUnchecked.ChangeType<SByte>(unchecked((UInt32)(-1))).ShouldBe((sbyte)-1);
            ConvertUnchecked.ChangeType<SByte>(UInt32.MinValue).ShouldBe((sbyte)0x00);
            /* ----                     SByte                              ---- */
            ConvertUnchecked.ChangeType<SByte>(unchecked((UInt64)0x8000)).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((UInt64)0x0000).ShouldBe((sbyte)0x00);
            ConvertUnchecked.ChangeType<SByte>((UInt64)0x0001).ShouldBe((sbyte)0x01);
            ConvertUnchecked.ChangeType<SByte>(unchecked((UInt64)(-1))).ShouldBe((sbyte)-1);
            ConvertUnchecked.ChangeType<SByte>(UInt64.MinValue).ShouldBe((sbyte)0x00);

            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<sbyte>(DateTime.Now));
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
            ConvertUnchecked.ChangeType<sbyte>(TestEnum.Value1).ShouldBe((sbyte)10);
            ConvertUnchecked.ChangeType<sbyte>(TestEnum.Value2).ShouldBe((sbyte) (-1));
        }
        #endregion

    }
}