using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNet.Plus.BasicType;
using DotNet.Plus.Endian;
using Shouldly;

namespace DotNet.Plus.Test.Endian
{
    [TestClass]
    public class Endianness32BitTests
    {
        [TestMethod]
        public void UInt32BigEndianTest()
        {
            int bufferOffset = 2;
            UInt32 bufferSourceValue = 0x12345678;
            var bufferSource = new byte[] { 0xFF, 0xFF, 0x12, 0x34, 0x56, 0x78, 0xFF, 0xFF };

            // Test ToUInt
            UInt32 value = bufferSource.ToUInt32(bufferOffset);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), startOffset: bufferOffset).ShouldBe(bufferSource);
            value.ToBufferNew().ShouldBe(new byte[] { 0x12, 0x34, 0x56, 0x78 });
        }

        [TestMethod]
        public void UInt32LittleEndianTest()
        {
            int bufferOffset = 1;
            UInt32 bufferSourceValue = 0x12345678;
            var bufferSource = new byte[] { 0xFF, 0x78, 0x56, 0x34, 0x12, 0xFF };

            // Test ToUInt
            UInt32 value = bufferSource.ToUInt32(bufferOffset, EndianFormat.Little);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment, endian: EndianFormat.Little).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), bufferOffset, EndianFormat.Little).ShouldBe(bufferSource);
            value.ToBufferNew(EndianFormat.Little).ShouldBe(new byte[] { 0x78, 0x56, 0x34, 0x12 });
        }

        [TestMethod]
        public void Int32BigEndianTest()
        {
            int bufferOffset = 2;
            Int32 bufferSourceValue = 0x12345678;
            var bufferSource = new byte[] { 0xFF, 0xFF, 0x12, 0x34, 0x56, 0x78, 0xFF, 0xFF };

            // Test ToInt
            Int32 value = bufferSource.ToInt32(bufferOffset);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), startOffset: bufferOffset).ShouldBe(bufferSource);
            value.ToBufferNew().ShouldBe(new byte[] { 0x12, 0x34, 0x56, 0x78 });
        }

        [TestMethod]
        public void Int32LittleEndianTest()
        {
            int bufferOffset = 1;
            Int32 bufferSourceValue = 0x12345678;
            var bufferSource = new byte[] { 0xFF, 0x78, 0x56, 0x34, 0x12, 0xFF };

            // Test ToInt
            Int32 value = bufferSource.ToInt32(bufferOffset, EndianFormat.Little);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment, endian: EndianFormat.Little).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), bufferOffset, EndianFormat.Little).ShouldBe(bufferSource);
            value.ToBufferNew(EndianFormat.Little).ShouldBe(new byte[] { 0x78, 0x56, 0x34, 0x12 });
        }
    }
}