using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNet.Plus.BasicType;
using DotNet.Plus.Endian;
using Shouldly;

namespace DotNet.Plus.Test.Endian
{
    [TestClass]
    public class Endianness64BitTests
    {
        [TestMethod]
        public void UInt64FailureTest()
        {
            var bufferSource = new byte[] { 0xFF, 0xFF, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0xFF, 0xFF };
            Should.Throw<ArgumentOutOfRangeException>(() => bufferSource.ToUInt64(100, 0, true));
        }

        [TestMethod]
        public void UInt64BigEndianTest()
        {
            int bufferOffset = 2;
            UInt64 bufferSourceValue = 0x123456789ABCDEF0;
            var bufferSource = new byte[] { 0xFF, 0xFF, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0xFF, 0xFF };

            // Test ToUInt
            UInt64 value = bufferSource.ToUInt64(bufferOffset);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), startOffset: bufferOffset).ShouldBe(bufferSource);
            value.ToBufferNew().ShouldBe(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 });
        }

        [TestMethod]
        public void UInt64LittleEndianTest()
        {
            int bufferOffset = 1;
            UInt64 bufferSourceValue = 0x123456789ABCDEF0;
            var bufferSource = new byte[] { 0xFF, 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12, 0xFF };

            // Test ToUInt
            UInt64 value = bufferSource.ToUInt64(bufferOffset, EndianFormat.Little);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment, endian: EndianFormat.Little).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), bufferOffset, EndianFormat.Little).ShouldBe(bufferSource);
            value.ToBufferNew(EndianFormat.Little).ShouldBe(new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 });
        }

        [TestMethod]
        public void Int64BigEndianTest()
        {
            int bufferOffset = 2;
            Int64 bufferSourceValue = 0x123456789ABCDEF0;
            var bufferSource = new byte[] { 0xFF, 0xFF, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0xFF, 0xFF };

            // Test ToInt
            Int64 value = bufferSource.ToInt64(bufferOffset);
            value.ShouldBe(bufferSourceValue);
            
            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), startOffset: bufferOffset).ShouldBe(bufferSource);
            value.ToBufferNew().ShouldBe(new byte[] { 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0 });
        }

        [TestMethod]
        public void Int64LittleEndianTest()
        {
            int bufferOffset = 1;
            Int64 bufferSourceValue = 0x123456789ABCDEF0;
            var bufferSource = new byte[] { 0xFF, 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12, 0xFF };

            // Test ToInt
            Int64 value = bufferSource.ToInt64(bufferOffset, EndianFormat.Little);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment, endian: EndianFormat.Little).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), bufferOffset, EndianFormat.Little).ShouldBe(bufferSource);
            value.ToBufferNew(EndianFormat.Little).ShouldBe(new byte[] { 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12 });
        }
    }
}