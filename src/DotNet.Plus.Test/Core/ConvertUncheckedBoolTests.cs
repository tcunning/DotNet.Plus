using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNet.Plus.Core;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class ConvertUncheckedBoolTests
    {
        [TestMethod]
        public void ChangeTypeBoolTest()
        {
            ConvertUnchecked.ChangeType<bool>(true).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(false).ShouldBe(false);

            ConvertUnchecked.ChangeType<bool>(Byte.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((byte)0x00).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>((byte)0x01).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(Byte.MinValue).ShouldBe(false);

            ConvertUnchecked.ChangeType<bool>(255M).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(0M).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>(1M).ShouldBe(true);

            ConvertUnchecked.ChangeType<bool>(decimal.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(255d).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(0d).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>(1d).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(-1d).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(decimal.MinValue).ShouldBe(true);

            ConvertUnchecked.ChangeType<bool>(Int16.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(unchecked((Int16)0x8000)).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((Int16)0x0000).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>((Int16)0x0001).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((Int16)(-1)).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(Int16.MinValue).ShouldBe(true);

            ConvertUnchecked.ChangeType<bool>(Int32.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(unchecked((Int32)0x8000)).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((Int32)0x0000).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>((Int32)0x0001).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((Int32)(-1)).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(Int32.MinValue).ShouldBe(true);

            ConvertUnchecked.ChangeType<bool>(Int64.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(unchecked((Int64)0x8000)).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((Int64)0x0000).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>((Int64)0x0001).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((Int64)(-1)).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(Int64.MinValue).ShouldBe(true);

            ConvertUnchecked.ChangeType<bool>(SByte.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((sbyte)0x00).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>((sbyte)0x01).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(SByte.MinValue).ShouldBe(true);

            ConvertUnchecked.ChangeType<bool>(float.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(255f).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(0f).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>(1f).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(-1f).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(float.MinValue).ShouldBe(true);

            ConvertUnchecked.ChangeType<bool>(UInt16.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(unchecked((UInt16)0x8000)).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((UInt16)0x0000).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>((UInt16)0x0001).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(unchecked((UInt16)(-1))).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(UInt16.MinValue).ShouldBe(false);

            ConvertUnchecked.ChangeType<bool>(UInt32.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(unchecked((UInt32)0x8000)).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((UInt32)0x0000).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>((UInt32)0x0001).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(unchecked((UInt32)(-1))).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(UInt32.MinValue).ShouldBe(false);

            ConvertUnchecked.ChangeType<bool>(UInt64.MaxValue).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(unchecked((UInt64)0x8000)).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>((UInt64)0x0000).ShouldBe(false);
            ConvertUnchecked.ChangeType<bool>((UInt64)0x0001).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(unchecked((UInt64)(-1))).ShouldBe(true);
            ConvertUnchecked.ChangeType<bool>(UInt64.MinValue).ShouldBe(false);

            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<bool>(DateTime.Now));
        }

        #region enum Test
        enum TestEnum
        {
            True = 1,
            False = 0
        }

        [TestMethod]
        public void ChangeTypeEnumTest()
        {
            ConvertUnchecked.ChangeType<bool>(TestEnum.True).ShouldBe(true);
        }
        #endregion

    }
}