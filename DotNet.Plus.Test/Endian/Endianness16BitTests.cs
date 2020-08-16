using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNet.Plus.BasicType;
using DotNet.Plus.Endian;
using Shouldly;

namespace DotNet.Plus.Test.Endian
{
    [TestClass]
    public class Endianness16BitTests
    {
        [TestMethod]
        public void UInt16BigEndianTest()
        {
            int bufferOffset = 2;
            UInt16 bufferSourceValue = 0x1234;
            var bufferSource = new byte[] { 0xFF, 0xFF, 0x12, 0x34, 0xFF, 0xFF };

            // Test ToUInt
            UInt16 value = bufferSource.ToUInt16(bufferOffset);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), startOffset: bufferOffset).ShouldBe(bufferSource);
            value.ToBufferNew().ShouldBe(new byte[] { 0x12, 0x34 });
        }

        [TestMethod]
        public void UInt16LittleEndianTest()
        {
            int bufferOffset = 1;
            UInt16 bufferSourceValue = 0x1234;
            var bufferSource = new byte[] { 0xFF, 0x34, 0x12, 0xFF };

            // Test ToUInt
            UInt16 value = bufferSource.ToUInt16(bufferOffset, EndianFormat.Little);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment, endian: EndianFormat.Little).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), bufferOffset, EndianFormat.Little).ShouldBe(bufferSource);
            value.ToBufferNew(EndianFormat.Little).ShouldBe(new byte[] { 0x34, 0x12 });
        }

        [TestMethod]
        public void Int16BigEndianTest()
        {
            int bufferOffset = 2;
            Int16 bufferSourceValue = 0x1234;
            var bufferSource = new byte[] { 0xFF, 0xFF, 0x12, 0x34, 0xFF, 0xFF };

            // Test ToInt
            Int16 value = bufferSource.ToInt16(bufferOffset);
            value.ShouldBe(bufferSourceValue);

            // Test Negative Numbers
            var negativeBufferSource = new byte[] { 0x80, 0x00 };
            negativeBufferSource.ToInt16(0).ShouldBe(Int16.MinValue);
            Int16.MinValue.ToBufferNew().ShouldBe(new byte[] { 0x80, 0x00});

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), startOffset: bufferOffset).ShouldBe(bufferSource);
            value.ToBufferNew().ShouldBe(new byte[] { 0x12, 0x34 });
        }

        [TestMethod]
        public void Int16LittleEndianTest()
        {
            int bufferOffset = 1;
            Int16 bufferSourceValue = 0x1234;
            var bufferSource = new byte[] { 0xFF, 0x34, 0x12, 0xFF };

            // Test ToInt
            Int16 value = bufferSource.ToInt16(bufferOffset, EndianFormat.Little);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(bufferOutputSegment, endian: EndianFormat.Little).Array.ShouldBe(bufferSource);
            value.ToBuffer(bufferOutput.Fill(0xFF), bufferOffset, EndianFormat.Little).ShouldBe(bufferSource);
            value.ToBufferNew(EndianFormat.Little).ShouldBe(new byte[] { 0x34, 0x12 });
        }
    }
}