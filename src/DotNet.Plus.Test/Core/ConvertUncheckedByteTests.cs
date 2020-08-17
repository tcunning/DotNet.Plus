using System;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class ConvertUncheckedByteTests
    {
        [TestMethod]
        public void ChangeTypeByteTest()
        {
            ConvertUnchecked.ChangeType<Byte>(true).ShouldBe((byte)1);
            ConvertUnchecked.ChangeType<Byte>(false).ShouldBe((byte)0);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(Byte.MaxValue).ShouldBe(Byte.MaxValue);
            ConvertUnchecked.ChangeType<Byte>((byte)0x00).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((byte)0x01).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>(Byte.MinValue).ShouldBe(Byte.MinValue);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(255M).ShouldBe((byte)255);
            ConvertUnchecked.ChangeType<Byte>(0M).ShouldBe((byte)0);
            ConvertUnchecked.ChangeType<Byte>(1M).ShouldBe((byte)1);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(double.MaxValue).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>(255d).ShouldBe((byte)255);
            ConvertUnchecked.ChangeType<Byte>(0d).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>(1d).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>(-1d).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(double.MinValue).ShouldBe((byte)0x00);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(Int16.MaxValue).ShouldBe(Byte.MaxValue);
            ConvertUnchecked.ChangeType<Byte>(unchecked((Int16)0x8000)).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((Int16)0x0000).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((Int16)0x0001).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>((Int16)(-1)).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(Int16.MinValue).ShouldBe((byte)0x00);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(Int32.MaxValue).ShouldBe(Byte.MaxValue);
            ConvertUnchecked.ChangeType<Byte>(unchecked((Int32)0x8000)).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((Int32)0x0000).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((Int32)0x0001).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>((Int32)(-1)).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(Int32.MinValue).ShouldBe((byte)0x00);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(Int64.MaxValue).ShouldBe(Byte.MaxValue);
            ConvertUnchecked.ChangeType<Byte>(unchecked((Int64)0x8000)).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((Int64)0x0000).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((Int64)0x0001).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>((Int64)(-1)).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(Int64.MinValue).ShouldBe((byte)0x00);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(SByte.MaxValue).ShouldBe((byte)SByte.MaxValue);
            ConvertUnchecked.ChangeType<Byte>((sbyte)0x00).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((sbyte)0x01).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>((sbyte)(-1)).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(SByte.MinValue).ShouldBe((byte)0x80);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(float.MaxValue).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>(255f).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(0f).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>(1f).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>(-1f).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(float.MinValue).ShouldBe((byte)0x00);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(UInt16.MaxValue).ShouldBe(Byte.MaxValue);
            ConvertUnchecked.ChangeType<Byte>(unchecked((UInt16)0x8000)).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((UInt16)0x0000).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((UInt16)0x0001).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>(unchecked((UInt16)(-1))).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(UInt16.MinValue).ShouldBe((byte)0x00);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(UInt32.MaxValue).ShouldBe(Byte.MaxValue);
            ConvertUnchecked.ChangeType<Byte>(unchecked((UInt32)0x8000)).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((UInt32)0x0000).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((UInt32)0x0001).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>(unchecked((UInt32)(-1))).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(UInt32.MinValue).ShouldBe((byte)0x00);
            /* ----                     Byte                              ---- */
            ConvertUnchecked.ChangeType<Byte>(UInt64.MaxValue).ShouldBe(Byte.MaxValue);
            ConvertUnchecked.ChangeType<Byte>(unchecked((UInt64)0x8000)).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((UInt64)0x0000).ShouldBe((byte)0x00);
            ConvertUnchecked.ChangeType<Byte>((UInt64)0x0001).ShouldBe((byte)0x01);
            ConvertUnchecked.ChangeType<Byte>(unchecked((UInt64)(-1))).ShouldBe((byte)0xFF);
            ConvertUnchecked.ChangeType<Byte>(UInt64.MinValue).ShouldBe((byte)0x00);

            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<Byte>(DateTime.Now));
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
            ConvertUnchecked.ChangeType<byte>(TestEnum.Value1).ShouldBe((byte)10);
            ConvertUnchecked.ChangeType<byte>(TestEnum.Value2).ShouldBe((byte)0xFF);
        }
        #endregion
    }
}