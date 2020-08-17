using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNet.Plus.BasicType;
using Shouldly;
using Byte = DotNet.Plus.BasicType.Byte;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class ByteArrayExDebugTests
    {
        [TestMethod]
        public void DebugDumpHexTest()
        {
            var data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

            data.DebugDump().ShouldBe("01 02 03 04 05 06 07 08 09 0a 0b 0c 0d 0e 0f");
            data.DebugDump(separator: ", ").ShouldBe("01, 02, 03, 04, 05, 06, 07, 08, 09, 0a, 0b, 0c, 0d, 0e, 0f");
        }

        [TestMethod]
        public void DebugDumpBinaryTest()
        {
            var data = new byte[] { 0xFF, 0xAA, 0xBB };

            data.DebugDump(startIndex: 1, toBase: Plus.BasicType.Byte.NumberBase.Binary).ShouldBe("10101010 10111011");
            data.DebugDump(startIndex: 1, separator: ", ", toBase: Plus.BasicType.Byte.NumberBase.Binary).ShouldBe("10101010, 10111011");
        }

        [TestMethod]
        public void DebugDumpInvalidBase()
        {
            var data = new byte[] { 0xAA, 0xBB };
            Should.Throw<ArgumentOutOfRangeException>(() => data.DebugDump(toBase: (Plus.BasicType.Byte.NumberBase) (-1)));
        }

        [TestMethod]
        public void DebugDumpArraySegmentTest()
        {
            var data = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

            var segment = new ArraySegment<byte>(data, 1, 2);
            segment.DebugDump().ShouldBe("02 03");
        }

    }
}