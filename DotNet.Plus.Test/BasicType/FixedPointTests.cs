using System;
using DotNet.Plus.BasicType;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class FixedPointTests
    {
        [TestMethod]
        public void ValueFloatByteFailureTest()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => float.NaN.MakeFixedPoint<byte>(5, 4));
            Should.Throw<ArgumentOutOfRangeException>(() => float.NaN.MakeFixedPoint<byte>(0, 0));

            Should.Throw<ArgumentOutOfRangeException>(() => FixedPoint.MakeFloat<byte>(0x0f, 5, 4));
            Should.Throw<ArgumentOutOfRangeException>(() => ((byte)0x0f).MakeFloat(0, 0));

            Should.Throw<OverflowException>(() => 18f.MakeFixedPoint<byte>(4, 0));

            Should.Throw<OverflowException>(() => (-1f).MakeFixedPoint<byte>(4, 0));

            Should.Throw<OverflowException>(() => ((float) FixedPoint.SmallestWholeValue(4, allowNegative: true) - 1).MakeFixedPoint<Int16>(4, 0));

            Should.Throw<OverflowException>(() => double.MaxValue.MakeFixedPoint<UInt64>(63, 0));
            Should.Throw<OverflowException>(() => float.MaxValue.MakeFixedPoint<UInt64>(63, 0));

            // From a raw numbers perspective this should not throw, but the real value is getting rounded when being
            // converted to a UInt64 because of loss of precision which is causing the conversion to overflow.
            //
            Should.Throw<OverflowException>(() => 9.2233720368547758E+18.MakeFixedPoint<UInt64>(63, 0).ShouldBe(0x7FFFFFFFFFFFFFFFul));

            Should.Throw<ArgumentOutOfRangeException>(() => FixedPoint.LargestWholeValue(100, allowNegative: true));

            Should.Throw<ArgumentOutOfRangeException>(() => FixedPoint.SmallestWholeValue(100, allowNegative: true));

            Should.Throw<ArgumentOutOfRangeException>(() => 10f.MakeFixedPoint<int>(100, fractionalBits: 0));

            FixedPoint.SmallestWholeValue(10, allowNegative: false).ShouldBe(0);
            FixedPoint.SmallestWholeValue(0, allowNegative: true).ShouldBe(0);

            Should.Throw<TypeInitializationException>(() => 2f.MakeFixedPoint<float>(4, 0)).InnerException.ShouldBeOfType<ArgumentException>();
        }

        [DataTestMethod]
        [DataRow(1f, (byte)0x10, (byte)4, (byte)4)]
        [DataRow(1.125f, (byte)0x12, (byte)4, (byte)4)]
        [DataRow(2.25f, (byte)0x12, (byte)4, (byte)3)]
        [DataRow(15f, (byte)0x0F, (byte)4, (byte)0)]         
        [DataRow(0.125f, (byte)0x02, (byte)0, (byte)4)]
        [DataRow(14.125f, (byte)0xF1, (byte)4, (byte)3)]
        public void ValueFloatByteTest(float realValue, byte fixedPointValue, byte wholeBits, byte fractionalBits)
        {
            // The fixedPointValue may have additional bits set which should be ignored when making the real value.  Only the
            // bits that make up the whole and fractional parts are considered.
            //
            fixedPointValue.MakeFloat(wholeBits, fractionalBits).ShouldBe(realValue);

            // Some of our test cases pass in "extra" data in the fixed point value.  This data is ignored, but it means those
            // bits will be 0 when converting from the real value to the FixedPoint so make the value we check again is properly
            // masked from the test data.
            //
            byte maskedFixedPointValue = (byte)(fixedPointValue & (byte)FixedPoint.FixedPointMask(wholeBits, fractionalBits));
            realValue.MakeFixedPoint<byte>(wholeBits, fractionalBits).ShouldBe(maskedFixedPointValue);
        }
        
        [DataTestMethod]
        [DataRow(1f, (UInt16)0x0100, (byte)8, (byte)8)]
        [DataRow(16f, (UInt16)0x0100, (byte)8, (byte)4)]
        public void ValueFloatUInt16Test(float realValue, UInt16 fixedPointValue, byte wholeBits, byte fractionalBits)
        {
            // The fixedPointValue may have additional bits set which should be ignored when making the real value.  Only the
            // bits that make up the whole and fractional parts are considered.
            //
            fixedPointValue.MakeFloat(wholeBits, fractionalBits).ShouldBe(realValue);

            // Some of our test cases pass in "extra" data in the fixed point value.  This data is ignored, but it means those
            // bits will be 0 when converting from the real value to the FixedPoint so make the value we check again is properly
            // masked from the test data.
            //
            UInt16 maskedFixedPointValue = (UInt16)(fixedPointValue & (UInt16)FixedPoint.FixedPointMask(wholeBits, fractionalBits));
            realValue.MakeFixedPoint<UInt16>(wholeBits, fractionalBits).ShouldBe(maskedFixedPointValue);
        }

        [DataTestMethod]
        [DataRow(1d, (UInt32)0x0100, (byte)8, (byte)8)]
        [DataRow(16d, (UInt32)0x0100, (byte)8, (byte)4)]
        [DataRow(4294967295d, (UInt32)0xFFFFFFFF, (byte)32, (byte)0)]
        public void ValueDoubleUInt64Test(double realValue, UInt32 fixedPointValue, byte wholeBits, byte fractionalBits)
        {
            // The fixedPointValue may have additional bits set which should be ignored when making the real value.  Only the
            // bits that make up the whole and fractional parts are considered.
            //
            fixedPointValue.MakeDouble(wholeBits, fractionalBits).ShouldBe(realValue, Double.Epsilon);

            // Some of our test cases pass in "extra" data in the fixed point value.  This data is ignored, but it means those
            // bits will be 0 when converting from the real value to the FixedPoint so make the value we check again is properly
            // masked from the test data.
            //
            UInt32 maskedFixedPointValue = (UInt32)(fixedPointValue & (UInt32)FixedPoint.FixedPointMask(wholeBits, fractionalBits));
            realValue.MakeFixedPoint<UInt32>(wholeBits, fractionalBits).ShouldBe(maskedFixedPointValue);
        }


        [DataTestMethod]
        [DataRow(1d, (UInt64)0x0100, (byte)8, (byte)8)]
        [DataRow(16d, (UInt64)0x0100, (byte)8, (byte)4)]
        [DataRow(9.223372e+18d, (UInt64)0x7FFFFFF76B48C000, (byte)63, (byte)0)]
        public void ValueDoubleUInt64Test(double realValue, UInt64 fixedPointValue, byte wholeBits, byte fractionalBits)
        {
            // The fixedPointValue may have additional bits set which should be ignored when making the real value.  Only the
            // bits that make up the whole and fractional parts are considered.
            //
            fixedPointValue.MakeDouble(wholeBits, fractionalBits).ShouldBe(realValue, Double.Epsilon);

            // Some of our test cases pass in "extra" data in the fixed point value.  This data is ignored, but it means those
            // bits will be 0 when converting from the real value to the FixedPoint so make the value we check again is properly
            // masked from the test data.
            //
            UInt64 maskedFixedPointValue = (UInt64)(fixedPointValue & (UInt64)FixedPoint.FixedPointMask(wholeBits, fractionalBits));
            realValue.MakeFixedPoint<UInt64>(wholeBits, fractionalBits).ShouldBe(maskedFixedPointValue);
        }
        
        [DataTestMethod]
        [DataRow(-1f, unchecked((Int16)0x0F00), (byte)4, (byte)8)]
        [DataRow(1f, (Int16)0x0100, (byte)8, (byte)8)]
        [DataRow(16f, (Int16)0x0100, (byte)8, (byte)4)]
        [DataRow(-1f, unchecked((Int16)0xFF00), (byte)8, (byte)8)]
        [DataRow(-1f, unchecked((Int16)0xFF00), (byte)7, (byte)8)]
        public void ValueFloatInt16Test(float realValue, Int16 fixedPointValue, byte wholeBits, byte fractionalBits)
        {
            // The fixedPointValue may have additional bits set which should be ignored when making the real value.  Only the
            // bits that make up the whole and fractional parts are considered.
            //
            fixedPointValue.MakeFloat(wholeBits, fractionalBits).ShouldBe(realValue);

            // Some of our test cases pass in "extra" data in the fixed point value.  This data is ignored, but it means those
            // bits will be 0 when converting from the real value to the FixedPoint so make the value we check again is properly
            // masked from the test data.
            //
            Int16 maskedFixedPointValue = (Int16)(fixedPointValue & (Int16)FixedPoint.FixedPointMask(wholeBits, fractionalBits));
            realValue.MakeFixedPoint<Int16>(wholeBits, fractionalBits).ShouldBe(maskedFixedPointValue);
        }

        [DataTestMethod]
        [DataRow(-1d, unchecked((Int16)0x0F00), (byte)4, (byte)8)]
        [DataRow(1d, (Int16)0x0100, (byte)8, (byte)8)]
        [DataRow(16d, (Int16)0x0100, (byte)8, (byte)4)]
        [DataRow(-1d, unchecked((Int16)0xFF00), (byte)8, (byte)8)]
        [DataRow(-1d, unchecked((Int16)0xFF00), (byte)7, (byte)8)]
        public void ValueDoubleInt16Test(double realValue, Int16 fixedPointValue, byte wholeBits, byte fractionalBits)
        {
            // The fixedPointValue may have additional bits set which should be ignored when making the real value.  Only the
            // bits that make up the whole and fractional parts are considered.
            //
            fixedPointValue.MakeDouble(wholeBits, fractionalBits).ShouldBe(realValue);

            // Some of our test cases pass in "extra" data in the fixed point value.  This data is ignored, but it means those
            // bits will be 0 when converting from the real value to the FixedPoint so make the value we check again is properly
            // masked from the test data.
            //
            Int16 maskedFixedPointValue = (Int16)(fixedPointValue & (Int16)FixedPoint.FixedPointMask(wholeBits, fractionalBits));
            realValue.MakeFixedPoint<Int16>(wholeBits, fractionalBits).ShouldBe(maskedFixedPointValue);
        }
    }
}