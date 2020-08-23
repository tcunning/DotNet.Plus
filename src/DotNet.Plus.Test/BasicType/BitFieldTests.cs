using System;
using DotNet.Plus.BasicType;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class BitFieldTests
    {
        [TestMethod]
        public void BitFieldByteTest()
        {
            var test = new BitField<byte>(0b0011_1100);
            test.Bitmask.ShouldBe<byte>(0b0011_1100);
            test.Decode(0b1111_1111).ShouldBe<byte>(0b1111);
            test.Decode(0b0000_1111).ShouldBe<byte>(0b0011);

            test.Encode(0b0000_0101, 0b0000_0000).ShouldBe<byte>(0b0001_0100);
            test.Encode(0b0000_0101, 0b1100_0000).ShouldBe<byte>(0b1101_0100);
            test.Encode(0b1111_0001, 0b0000_0000).ShouldBe<byte>(0b0000_0100);
        }

        [TestMethod]
        public void BitFieldMultiByteTest()
        {
            var test = new BitField<byte, ushort>(4, startBitOffset: 6);
            test.Bitmask.ShouldBe<ushort>(0b0000_0011_1100_0000);
            test.Decode(0b0000_0011_1100_0000).ShouldBe<byte>(0b1111);
            test.Decode(0b0000_0010_0100_0000).ShouldBe<byte>(0b1001);

            test.Encode(0b1111, 0b0000_0000_0000_0000).ShouldBe<ushort>(0b0000_0011_1100_0000);
            test.Encode(0b0000, 0b1111_1111_1111_1111).ShouldBe<ushort>(0b1111_1100_0011_1111);
        }

        [TestMethod]
        public void BitFieldMultiByte2Test()
        {
            var test = new BitField<ushort, ushort>(4, startBitOffset: 12);
            test.Bitmask.ShouldBe<ushort>(0b0000_0000_0000_1111);
            test.Decode(0b1111_1111_1111_1010).ShouldBe<ushort>(0b1010);
        }

        [TestMethod]
        public void BitFieldMultiByte3Test()
        {
            var test = new BitField<ushort, ushort>(4, startBitOffset: 0);
            test.Bitmask.ShouldBe<ushort>(0b1111_0000_0000_0000);
            test.Decode(0b1010_1111_1111_1111).ShouldBe<ushort>(0b1010);
        }

        [TestMethod]
        public void BitFieldErrorTest()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => new BitField<byte, ushort>(9, startBitOffset: 0));
            Should.Throw<ArgumentOutOfRangeException>(() => new BitField<byte, ushort>(8, startBitOffset: 9));
        }

        [TestMethod]
        public void BitFieldTest()
        {
            (new BitField<byte>(0b0000_0000)).Bitmask.ShouldBe<byte>(0b0000_0000);
            (new BitField<byte>(0b1111_1111)).Bitmask.ShouldBe<byte>(0b1111_1111);
            (new BitField<byte>(0b1000_0000)).Bitmask.ShouldBe<byte>(0b1000_0000);
            (new BitField<byte>(0b0000_0001)).Bitmask.ShouldBe<byte>(0b0000_0001);

            Should.Throw<ArgumentOutOfRangeException>(() => new BitField<byte>(0b1000_0001));
            Should.Throw<ArgumentOutOfRangeException>(() => new BitField<byte>(0b0110_0010));
            Should.Throw<ArgumentOutOfRangeException>(() => new BitField<byte>(0b0101_1100));
        }

    }
}