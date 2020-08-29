using DotNet.Plus.BasicType;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class BitFieldBoolTests
    {
        [TestMethod]
        public void BitFieldBoolLsbTest()
        {
            var test = new BitFieldBoolean<byte>(0);
            test.Bitmask.ShouldBe<byte>(0b0000_0001);

            test.Decode(0b1111_1111).ShouldBe<bool>(true);
            test.HasFlag(0b1111_1111).ShouldBe(true);

            test.Decode(0b0000_1111).ShouldBe<bool>(true);
            test.HasFlag(0b0000_1111).ShouldBe(true);

            test.Decode(0b1111_1110).ShouldBe<bool>(false);
            test.HasFlag(0b1111_1110).ShouldBe(false);

            test.Encode(false, 0b1111_1111).ShouldBe<byte>(0b1111_1110);
            test.Encode(true, 0b0000_0000).ShouldBe<byte>(0b0000_0001);
            test.Encode(true, 0b1111_1111).ShouldBe<byte>(0b1111_1111);
            test.Encode(true, 0b1111_0000).ShouldBe<byte>(0b1111_0001);
        }

        [TestMethod]
        public void BitFieldBoolSetFlagsTest()
        {
            var test = BitFieldBoolean<byte>.MakeFromBitmask(0b0001_0000);
            test.Bitmask.ShouldBe<byte>(0b0001_0000);

            test.SetFlag(0b0000_0000).ShouldBe<byte>(0b0001_0000);
            test.ClearFlag(0b0000_0000).ShouldBe<byte>(0b0000_0000);
            test.ClearFlag(0b1111_1111).ShouldBe<byte>(0b1110_1111);
        }

        [TestMethod]
        public void BitFieldBoolUpdateFlagsTest()
        {
            var test = BitFieldBoolean<byte>.MakeFromBitmask(0b0000_0001);
            test.Bitmask.ShouldBe<byte>(0b0000_0001);

            test.UpdateFlag(true, 0b0000_0000).ShouldBe<byte>(0b0000_0001);
            test.UpdateFlag(false, 0b0000_0000).ShouldBe<byte>(0b0000_0000);
            test.UpdateFlag(false, 0b1111_1111).ShouldBe<byte>(0b1111_1110);
        }

        [TestMethod]
        public void BitFieldBoolMsbTest()
        {
            var test = BitFieldBoolean<byte>.MakeFromBitmask(0b1000_0000);
            test.Bitmask.ShouldBe<byte>(0b1000_0000);

            test.Decode(0b1111_1111).ShouldBe<bool>(true);
            test.Decode(0b0000_1111).ShouldBe<bool>(false);
            test.Decode(0b1111_1110).ShouldBe<bool>(true);

            test.Encode(false, 0b1111_1111).ShouldBe<byte>(0b0111_1111);
            test.Encode(true, 0b0000_0000).ShouldBe<byte>(0b1000_0000);
            test.Encode(true, 0b1111_1111).ShouldBe<byte>(0b1111_1111);
            test.Encode(true, 0b1111_0000).ShouldBe<byte>(0b1111_0000);
        }

        [TestMethod]
        public void BitFieldBoolErrorTest()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => new BitFieldBoolean<byte>(8));
            Should.Throw<ArgumentOutOfRangeException>(() => BitFieldBoolean<byte>.MakeFromBitmask(0b1100_0000));
            Should.Throw<ArgumentOutOfRangeException>(() => BitFieldBoolean<byte>.MakeFromBitmask(0b0000_0000));
        }

        [TestMethod]
        public void BitFieldBoolConversionTest()
        {
            var test1 = BitFieldBoolean<byte>.MakeFromBitmask(0b1000_0000);
            BitField<bool, byte> test2 = test1;
            test1.Bitmask.ShouldBe(test2.Bitmask);

            BitFieldBoolean<byte> test3 = test2;
            test2.Bitmask.ShouldBe(test3.Bitmask);
            test1.Bitmask.ShouldBe(test2.Bitmask);
        }

        [TestMethod]
        public void BitFieldBooleanBufferDecodeTest()
        {
            BitFieldBoolean<byte>.MakeFromBitmask(0b1000_0000).Decode(new byte[] { 0x80, 0x00 }, 0).ShouldBe(true);
            BitFieldBoolean<byte>.MakeFromBitmask(0b0000_0001).Decode(new byte[] { 0x00, 0x01 }, 1).ShouldBe(true);
            BitFieldBoolean<byte>.MakeFromBitmask(0b0000_0100).Decode(new byte[] { 0x80, 0x04 }, 0).ShouldBe(false);
            BitFieldBoolean<byte>.MakeFromBitmask(0b0000_0100).Decode(new byte[] { 0x80, 0x04 }, 1).ShouldBe(true);
        }

        [TestMethod]
        public void BitFieldBooleanBufferEncodeTest()
        {
            BitFieldBoolean<byte>.MakeFromBitmask(0b1000_0000).Encode(true, new byte[] { 0x00, 0x00 }).ShouldBe(new byte[] { 0x80, 0x00 });
            BitFieldBoolean<byte>.MakeFromBitmask(0b1000_0000).Encode(true, new byte[] { 0x00, 0x00 }, offset: 1).ShouldBe(new byte[] { 0x00, 0x80 });
            BitFieldBoolean<byte>.MakeFromBitmask(0b0001_0000).Encode(false, new byte[] { 0xFF, 0xFF }).ShouldBe(new byte[] { 0xEF, 0xFF });
        }

    }

}