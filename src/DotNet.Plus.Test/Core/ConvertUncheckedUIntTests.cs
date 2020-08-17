using System;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class ConvertUncheckedUIntTests
    {
        [TestMethod]
        public void ChangeTypeUInt16Test()
        {
            ConvertUnchecked.ChangeType<UInt16>(true).ShouldBe((UInt16)1);
            ConvertUnchecked.ChangeType<UInt16>(false).ShouldBe((UInt16)0);
            ConvertUnchecked.ChangeType<UInt16>((byte)0xAA).ShouldBe((UInt16)0xAA);
            ConvertUnchecked.ChangeType<UInt16>(255M).ShouldBe((UInt16)255);
            ConvertUnchecked.ChangeType<UInt16>(1.1234d).ShouldBe((UInt16)(1.1234));
            ConvertUnchecked.ChangeType<UInt16>((Int16)0x1234).ShouldBe((UInt16)0x1234);
            ConvertUnchecked.ChangeType<UInt16>((Int32)0x12345678).ShouldBe((UInt16)0x5678);
            ConvertUnchecked.ChangeType<UInt16>((Int64)0xABCDEF12345678).ShouldBe((UInt16)0x5678);
            ConvertUnchecked.ChangeType<UInt16>((sbyte)(0x7F)).ShouldBe((UInt16)(0x7F));
            ConvertUnchecked.ChangeType<UInt16>(255f).ShouldBe((UInt16)0x00FF);
            ConvertUnchecked.ChangeType<UInt16>((UInt16)0x1234).ShouldBe((UInt16)0x1234);
            ConvertUnchecked.ChangeType<UInt16>(unchecked((UInt32)(-1))).ShouldBe((UInt16)(0xFFFF));
            ConvertUnchecked.ChangeType<UInt16>(unchecked((UInt64)(-1))).ShouldBe((UInt16)(0xFFFF));
            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<UInt16>(DateTime.Now));
        }

        [TestMethod]
        public void ChangeTypeUInt32Test()
        {
            ConvertUnchecked.ChangeType<UInt32>(true).ShouldBe((UInt32)1);
            ConvertUnchecked.ChangeType<UInt32>(false).ShouldBe((UInt32)0);
            ConvertUnchecked.ChangeType<UInt32>((byte)0xAA).ShouldBe((UInt32)0xAA);
            ConvertUnchecked.ChangeType<UInt32>(255M).ShouldBe((UInt32)255);
            ConvertUnchecked.ChangeType<UInt32>(1.1234d).ShouldBe((UInt32)(1.1234));
            ConvertUnchecked.ChangeType<UInt32>((Int16)0x1234).ShouldBe((UInt32)0x1234);
            ConvertUnchecked.ChangeType<UInt32>((Int32)0x12345678).ShouldBe((UInt32)0x12345678);
            ConvertUnchecked.ChangeType<UInt32>((Int64)0xABCDEF12345678).ShouldBe((UInt32)0x12345678);
            ConvertUnchecked.ChangeType<UInt32>((sbyte)(0x7F)).ShouldBe((UInt32)(0x7F));
            ConvertUnchecked.ChangeType<UInt32>(255f).ShouldBe((UInt32)0x00FF);
            ConvertUnchecked.ChangeType<UInt32>((UInt16)0x1234).ShouldBe((UInt32)0x1234);
            ConvertUnchecked.ChangeType<UInt32>(unchecked((UInt32)(-1))).ShouldBe((UInt32)0xFFFFFFFF);
            ConvertUnchecked.ChangeType<UInt32>(unchecked((UInt64)(-1))).ShouldBe((UInt32)0xFFFFFFFF);
            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<UInt32>(DateTime.Now));
        }

        [TestMethod]
        public void ChangeTypeUInt64Test()
        {
            ConvertUnchecked.ChangeType<UInt64>(true).ShouldBe((UInt64)1);
            ConvertUnchecked.ChangeType<UInt64>(false).ShouldBe((UInt64)0);
            ConvertUnchecked.ChangeType<UInt64>((byte)0xAA).ShouldBe((UInt64)0xAA);
            ConvertUnchecked.ChangeType<UInt64>(255M).ShouldBe((UInt64)255);
            ConvertUnchecked.ChangeType<UInt64>(1.1234d).ShouldBe((UInt64)(1.1234));
            ConvertUnchecked.ChangeType<UInt64>((Int16)0x1234).ShouldBe((UInt64)0x1234);
            ConvertUnchecked.ChangeType<UInt64>((Int32)0x12345678).ShouldBe((UInt64)0x12345678);
            ConvertUnchecked.ChangeType<UInt64>((Int64)0xABCDEF12345678).ShouldBe((UInt64)0xABCDEF12345678);
            ConvertUnchecked.ChangeType<UInt64>((sbyte)(0x7F)).ShouldBe((UInt64)(0x7F));
            ConvertUnchecked.ChangeType<UInt64>(255f).ShouldBe((UInt64)0x00FF);
            ConvertUnchecked.ChangeType<UInt64>((UInt16)0x1234).ShouldBe((UInt64)0x1234);
            ConvertUnchecked.ChangeType<UInt64>((UInt32)0x1234_5678).ShouldBe((UInt64)0x1234_5678);
            ConvertUnchecked.ChangeType<UInt64>(unchecked((Int64)(-1))).ShouldBe((UInt64)0xFFFFFFFFFFFFFFFF);
            ConvertUnchecked.ChangeType<UInt64>(unchecked((UInt64)(-1))).ShouldBe((UInt64)0xFFFFFFFFFFFFFFFF);
            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<UInt64>(DateTime.Now));
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
            ConvertUnchecked.ChangeType<UInt16>(TestEnum.Value1).ShouldBe((UInt16)10);
            ConvertUnchecked.ChangeType<UInt16>(TestEnum.Value2).ShouldBe((UInt16)0xFF);

            ConvertUnchecked.ChangeType<UInt32>(TestEnum.Value1).ShouldBe((UInt32)10);
            ConvertUnchecked.ChangeType<UInt32>(TestEnum.Value2).ShouldBe((UInt32)0xFF);

            ConvertUnchecked.ChangeType<UInt64>(TestEnum.Value1).ShouldBe((UInt64)10);
            ConvertUnchecked.ChangeType<UInt64>(TestEnum.Value2).ShouldBe((UInt64)0xFF);
        }
        #endregion
    }
}