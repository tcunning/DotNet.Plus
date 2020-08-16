using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using DotNet.Plus.BasicType;
using Shouldly;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class IntegerTests
    {
        public TFixedPointValue Testing<TFixedPointValue>(int other)
            where TFixedPointValue : struct, IConvertible
        {
            Integer<TFixedPointValue> test = other; 
            return test.Value;
        }

        public enum Test : short { A = -1, B = 0, C = 1 }

        [TestMethod]
        public void BasicTest()
        {
            Integer<uint> test = uint.MaxValue;
            Integer<uint> test2 = new Integer<uint>(test);
            Integer<UInt16> test3 = 0x1234;
            Integer<UInt16> test4 = new Integer<UInt16>(test3);

            test.Value.ShouldBe(uint.MaxValue);
            ((Int32)test2).ShouldBe(-1);

            Integer<ushort> testEnum = Test.A;
            testEnum.Value.ShouldBe((UInt16)0xFFFF);

            Integer<Test> testEnum2 = Test.A;
            testEnum2.ShouldBe(Test.A);

            Integer<UInt16> test5 = 100;
            ((sbyte)test5).ShouldBe((sbyte)100);
            ((Int16)test5).ShouldBe((Int16)100);
            ((byte)test5).ShouldBe((byte)100);
            ((float)test5).ShouldBe((float)100);
            ((double)test5).ShouldBe((double)100);
            ((Decimal)test5).ShouldBe((Decimal)100);

            ((Integer<sbyte>)((sbyte)100)).Value.ShouldBe((sbyte)100);
            ((Integer<Int16>)((Int16)100)).Value.ShouldBe((Int16)100);
            ((Integer<byte>)((byte)100)).Value.ShouldBe((byte)100);
            ((Integer<UInt16>)((UInt16)100)).Value.ShouldBe((UInt16)100);
        }
    }
}