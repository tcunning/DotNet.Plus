using System;
using DotNet.Plus.BasicType;
using DotNet.Plus.Endian;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class GuidUtilTests
    {
        [TestMethod]
        public void ToGuidToSmallTest()
        {
            var invalidGuid = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
            Should.Throw<ArgumentOutOfRangeException>(() => invalidGuid.ToGuid(startOffset: 0));
        }

        [TestMethod]
        public void ToGuidBigEndianTest()
        {
            var guidBytes = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };
            var guid = guidBytes.ToGuid();
            guid.ShouldBe(new Guid("01020304-0506-0708-090a-0b0c0d0e0f10"));
        }

        [TestMethod]
        public void ToGuidLittleTest()
        {
            var guidBytes = new byte[] { 0xFF, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F, 0x10 };
            var guid = guidBytes.ToGuid(startOffset: 1, endian: EndianFormat.Little);
            guid.ShouldBe(new Guid("04030201-0605-0807-090a-0b0c0d0e0f10"));
        }
    }
}