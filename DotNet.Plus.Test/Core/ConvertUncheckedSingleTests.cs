using System;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class ConvertUncheckedSingleTests
    {
        [TestMethod]
        public void ChangeTypeSingleTest()
        {
            ConvertUnchecked.ChangeType<float>(true).ShouldBe((float)1);
            ConvertUnchecked.ChangeType<float>(false).ShouldBe((float)0);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>(Byte.MaxValue).ShouldBe((float)255);
            ConvertUnchecked.ChangeType<float>((decimal)0x00).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>((decimal)0x01).ShouldBe((float)0x01);
            ConvertUnchecked.ChangeType<float>(Byte.MinValue).ShouldBe((float)0x00);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>(255M).ShouldBe((float)255);
            ConvertUnchecked.ChangeType<float>(0M).ShouldBe((float)0);
            ConvertUnchecked.ChangeType<float>(1M).ShouldBe((float)1);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>(255d).ShouldBe((float)255);
            ConvertUnchecked.ChangeType<float>(0d).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>(1d).ShouldBe((float)0x01);
            ConvertUnchecked.ChangeType<float>(-1d).ShouldBe((float)-1);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>((Int16)0x0000).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>((Int16)0x0001).ShouldBe((float)0x01);
            ConvertUnchecked.ChangeType<float>((Int16)(-1)).ShouldBe((float)-1);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>((Int32)0x0000).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>((Int32)0x0001).ShouldBe((float)0x01);
            ConvertUnchecked.ChangeType<float>((Int32)(-1)).ShouldBe((float)-1);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>((Int64)0x0000).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>((Int64)0x0001).ShouldBe((float)0x01);
            ConvertUnchecked.ChangeType<float>((Int64)(-1)).ShouldBe((float)-1);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>((sbyte)0x00).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>((sbyte)0x01).ShouldBe((float)0x01);
            ConvertUnchecked.ChangeType<float>((sbyte)(-1)).ShouldBe((float)-1);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>(255f).ShouldBe((float)0xFF);
            ConvertUnchecked.ChangeType<float>(0f).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>(1f).ShouldBe((float)0x01);
            ConvertUnchecked.ChangeType<float>(-1f).ShouldBe((float)-1);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>((UInt16)0x0000).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>((UInt16)0x0001).ShouldBe((float)0x01);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>((UInt32)0x0000).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>((UInt32)0x0001).ShouldBe((float)0x01);
            ConvertUnchecked.ChangeType<float>(UInt32.MinValue).ShouldBe((float)0x00);
            /* ----                     float                           ---- */
            ConvertUnchecked.ChangeType<float>((UInt64)0x0000).ShouldBe((float)0x00);
            ConvertUnchecked.ChangeType<float>((UInt64)0x0001).ShouldBe((float)0x01);

            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<float>(DateTime.Now));
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
            ConvertUnchecked.ChangeType<float>(TestEnum.Value1).ShouldBe(10.0f);
            ConvertUnchecked.ChangeType<float>(TestEnum.Value2).ShouldBe(255.0f);
        }
        #endregion

    }
}