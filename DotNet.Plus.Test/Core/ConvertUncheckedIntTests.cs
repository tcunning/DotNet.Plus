using System;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class ConvertUncheckedIntTests
    {
        [TestMethod]
        public void ChangeTypeInt16Test()
        {
            ConvertUnchecked.ChangeType<Int16>(true).ShouldBe((Int16)1);
            ConvertUnchecked.ChangeType<Int16>(false).ShouldBe((Int16)0);
            ConvertUnchecked.ChangeType<Int16>((byte)0xAA).ShouldBe((Int16)0xAA);
            ConvertUnchecked.ChangeType<Int16>(255M).ShouldBe((Int16)255);
            ConvertUnchecked.ChangeType<Int16>(-1.1234d).ShouldBe((Int16)(-1.1234));
            ConvertUnchecked.ChangeType<Int16>((Int16)0x1234).ShouldBe((Int16)0x1234);
            ConvertUnchecked.ChangeType<Int16>((Int32)0x12345678).ShouldBe((Int16)0x5678);
            ConvertUnchecked.ChangeType<Int16>((Int64)0xABCDEF12345678).ShouldBe((Int16)0x5678);
            ConvertUnchecked.ChangeType<Int16>((sbyte)(-1)).ShouldBe((Int16)(-1));
            ConvertUnchecked.ChangeType<Int16>(255f).ShouldBe((Int16)0x00FF);
            ConvertUnchecked.ChangeType<Int16>((UInt16)0x1234).ShouldBe((Int16)0x1234);
            ConvertUnchecked.ChangeType<Int16>(unchecked((UInt32)(-1))).ShouldBe((Int16)(-1));
            ConvertUnchecked.ChangeType<Int16>(unchecked((UInt64)(-1))).ShouldBe((Int16)(-1));
            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<Int16>(DateTime.Now));
        }

        [TestMethod]
        public void ChangeTypeInt32Test()
        {
            ConvertUnchecked.ChangeType<Int32>(true).ShouldBe((Int32)1);
            ConvertUnchecked.ChangeType<Int32>(false).ShouldBe((Int32)0);
            ConvertUnchecked.ChangeType<Int32>((byte)0xAA).ShouldBe((Int32)0xAA);
            ConvertUnchecked.ChangeType<Int32>(255M).ShouldBe((Int32)255);
            ConvertUnchecked.ChangeType<Int32>(-1.1234d).ShouldBe((Int32)(-1.1234));
            ConvertUnchecked.ChangeType<Int32>((Int16)0x1234).ShouldBe((Int32)0x1234);
            ConvertUnchecked.ChangeType<Int32>((Int32)0x12345678).ShouldBe((Int32)0x12345678);
            ConvertUnchecked.ChangeType<Int32>((Int64)0xABCDEF12345678).ShouldBe((Int32)0x12345678);
            ConvertUnchecked.ChangeType<Int32>((sbyte)(-1)).ShouldBe((Int32)(-1));
            ConvertUnchecked.ChangeType<Int32>(255f).ShouldBe((Int32)0x00FF);
            ConvertUnchecked.ChangeType<Int32>((UInt16)0x1234).ShouldBe((Int32)0x1234);
            ConvertUnchecked.ChangeType<Int32>(unchecked((UInt32)(-1))).ShouldBe((Int32)(-1));
            ConvertUnchecked.ChangeType<Int32>(unchecked((UInt64)(-1))).ShouldBe((Int32)(-1));
            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<Int32>(DateTime.Now));
        }

        [TestMethod]
        public void ChangeTypeInt64Test()
        {
            ConvertUnchecked.ChangeType<Int64>(true).ShouldBe((Int64)1);
            ConvertUnchecked.ChangeType<Int64>(false).ShouldBe((Int64)0);
            ConvertUnchecked.ChangeType<Int64>((byte)0xAA).ShouldBe((Int64)0xAA);
            ConvertUnchecked.ChangeType<Int64>(255M).ShouldBe((Int64)255);
            ConvertUnchecked.ChangeType<Int64>(-1.1234d).ShouldBe((Int64)(-1.1234));
            ConvertUnchecked.ChangeType<Int64>((Int16)0x1234).ShouldBe((Int64)0x1234);
            ConvertUnchecked.ChangeType<Int64>((Int32)0x12345678).ShouldBe((Int64)0x12345678);
            ConvertUnchecked.ChangeType<Int64>((Int64)0xABCDEF12345678).ShouldBe((Int64)0xABCDEF12345678);
            ConvertUnchecked.ChangeType<Int64>((sbyte)(-1)).ShouldBe((Int64)(-1));
            ConvertUnchecked.ChangeType<Int64>(255f).ShouldBe((Int64)0x00FF);
            ConvertUnchecked.ChangeType<Int64>((UInt16)0x1234).ShouldBe((Int64)0x1234);
            ConvertUnchecked.ChangeType<Int64>((UInt32)0x1234_5678).ShouldBe((Int64)0x1234_5678);
            ConvertUnchecked.ChangeType<Int64>(unchecked((Int64)(-1))).ShouldBe((Int64)(-1));
            ConvertUnchecked.ChangeType<Int64>(unchecked((UInt64)(-1))).ShouldBe((Int64)(-1));
            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<Int64>(DateTime.Now));
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
            ConvertUnchecked.ChangeType<Int16>(TestEnum.Value1).ShouldBe((Int16)10);
            ConvertUnchecked.ChangeType<Int16>(TestEnum.Value2).ShouldBe((Int16)0xFF);

            ConvertUnchecked.ChangeType<Int32>(TestEnum.Value1).ShouldBe((Int16)10);
            ConvertUnchecked.ChangeType<Int32>(TestEnum.Value2).ShouldBe((Int16)0xFF);

            ConvertUnchecked.ChangeType<Int64>(TestEnum.Value1).ShouldBe((Int16)10);
            ConvertUnchecked.ChangeType<Int64>(TestEnum.Value2).ShouldBe((Int16)0xFF);
        }
        #endregion
    }
}