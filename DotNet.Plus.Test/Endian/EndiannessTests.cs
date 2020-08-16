using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Plus.BasicType;
using System;
using DotNet.Plus.Endian;
using Shouldly;

namespace DotNet.Plus.Test.Endian
{
    [TestClass]
    public class EndiannessTests
    {
        [TestMethod]
        public void ValueBigEndianTest()
        {
            int bufferOffset = 2;
            UInt64 bufferSourceValue = 0x123456789ABCDEF0;
            var bufferSource = new byte[] { 0xFF, 0xFF, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0xFF, 0xFF };

            // Test ToUInt
            UInt64 value = bufferSource.ToUInt64(sizeof(UInt64), bufferOffset, signExtend: false);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(sizeof(UInt64), bufferOutputSegment).Array.ShouldBe(bufferSource);
        }

        [TestMethod]
        public void ValueLittleEndianTest()
        {
            int bufferOffset = 1;
            UInt64 bufferSourceValue = 0x123456789ABCDEF0;
            var bufferSource = new byte[] { 0xFF, 0xF0, 0xDE, 0xBC, 0x9A, 0x78, 0x56, 0x34, 0x12, 0xFF };

            // Test ToUInt
            UInt64 value = bufferSource.ToUInt64(sizeof(UInt64), bufferOffset, signExtend: false, EndianFormat.Little);
            value.ShouldBe(bufferSourceValue);

            // Test ToBuffer
            var bufferOutput = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            var bufferOutputSegment = new ArraySegment<byte>(bufferOutput, bufferOffset, bufferOutput.Length - bufferOffset);
            value.ToBuffer(sizeof(UInt64), bufferOutputSegment, endian: EndianFormat.Little).Array.ShouldBe(bufferSource);
        }

        [TestMethod]
        public void NegativeValueTest()
        {
            var bufferSource = new byte[] { 0x80, 0x00, 0x00 };  // Represents -8388608 for a 24 bit number
            Int32 max24BitNegativeSigned = -8388608;
            UInt32 max24BitNegativeUnSigned = 0xFF80_0000;

            bufferSource.ToUInt64(3, 0, signExtend: false).ShouldBe(0x0080_0000uL);
            bufferSource.ToUInt64(3, 0, signExtend: true).ShouldBe(0xFFFF_FFFF_FF80_0000ul);
            bufferSource.ToInt64(3, 0).ShouldBe(0xFFFF_FFFF_FF80_0000ul);
            ((Int32)bufferSource.ToInt64(3, 0)).ShouldBe(max24BitNegativeSigned);
            ((UInt32)bufferSource.ToUInt64(3, 0, signExtend: true)).ShouldBe(max24BitNegativeUnSigned);
        }

        [TestMethod]
        public void ValueArgumentTest()
        {
            int size = sizeof(UInt64);
            byte[] buffer = new byte[size];

            Should.Throw<ArgumentOutOfRangeException>(() => buffer.ToUInt64(-1, 0, signExtend: false));
            Should.Throw<ArgumentOutOfRangeException>(() => buffer.ToUInt64(20, 0, signExtend: false));

            byte[] inputZero = new byte[0];
            Should.Throw<ArgumentOutOfRangeException>(() => inputZero.ToUInt64(4, 0, signExtend: false));
            inputZero.ToUInt64(0, 0, signExtend: false).ShouldBe(0uL);

            byte[] inputSmall = new byte[6];
            Should.Throw<ArgumentOutOfRangeException>(() => inputSmall.ToUInt64(7, 0, signExtend: false));
        }

        [TestMethod]
        public void BufferArgumentTest()
        {
            UInt64 bufferSourceValue = 0x123456789ABCDEF0;
            int size = sizeof(UInt64);
            byte[] buffer = new byte[size];

            var bufferUnchanged = (new byte[size]).Fill(0xFF);
            buffer.Fill(0xFF);
            bufferSourceValue.ToBuffer(0, new ArraySegment<byte>(buffer, 0, size)).Array.ShouldBe(bufferUnchanged);
            
            Should.Throw<ArgumentOutOfRangeException>(() => bufferSourceValue.ToBuffer(-1, new ArraySegment<byte>(buffer, 0, 0)));
            Should.Throw<ArgumentOutOfRangeException>(() => bufferSourceValue.ToBuffer(-1, new ArraySegment<byte>(buffer, 0, size)));
            Should.Throw<ArgumentOutOfRangeException>(() => bufferSourceValue.ToBuffer(size+1, new ArraySegment<byte>(buffer, 0, size)));
            Should.Throw<ArgumentOutOfRangeException>(() => bufferSourceValue.ToBuffer(size, new ArraySegment<byte>(buffer, 0, -1)));

            byte[] bufferToSmall = new byte[size-1];
            Should.Throw<ArgumentOutOfRangeException>(() => bufferSourceValue.ToBuffer(size, new ArraySegment<byte>(bufferToSmall, 0, size - 1)));
        }
    }
}