using System;
using DotNet.Plus.BasicType;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class IntegerDefinitionTests
    {
        [TestMethod]
        public void IntegerDefinitionTest()
        {
            IntegerDefinition<bool>.TypeCode.ShouldBe(TypeCode.Boolean);
            IntegerDefinition<sbyte>.TypeCode.ShouldBe(TypeCode.SByte);
            IntegerDefinition<Int16>.TypeCode.ShouldBe(TypeCode.Int16);
            IntegerDefinition<Int32>.TypeCode.ShouldBe(TypeCode.Int32);
            IntegerDefinition<Int64>.TypeCode.ShouldBe(TypeCode.Int64);
            IntegerDefinition<byte>.TypeCode.ShouldBe(TypeCode.Byte);
            IntegerDefinition<UInt16>.TypeCode.ShouldBe(TypeCode.UInt16);
            IntegerDefinition<UInt32>.TypeCode.ShouldBe(TypeCode.UInt32);
            IntegerDefinition<UInt64>.TypeCode.ShouldBe(TypeCode.UInt64);
        }

        [TestMethod]
        public void IntegerDefinitionSignTest()
        {
            IntegerDefinition<bool>.IsSigned.ShouldBe(false);
            IntegerDefinition<sbyte>.IsSigned.ShouldBe(true);
            IntegerDefinition<Int16>.IsSigned.ShouldBe(true);
            IntegerDefinition<Int32>.IsSigned.ShouldBe(true);
            IntegerDefinition<Int64>.IsSigned.ShouldBe(true);
            IntegerDefinition<byte>.IsSigned.ShouldBe(false);
            IntegerDefinition<UInt16>.IsSigned.ShouldBe(false);
            IntegerDefinition<UInt32>.IsSigned.ShouldBe(false);
            IntegerDefinition<UInt64>.IsSigned.ShouldBe(false);
        }

        [TestMethod]
        public void IntegerDefinitionSizeTest()
        {
            IntegerDefinition<bool>.Size.ShouldBe(1);
            IntegerDefinition<sbyte>.Size.ShouldBe(1);
            IntegerDefinition<Int16>.Size.ShouldBe(2);
            IntegerDefinition<Int32>.Size.ShouldBe(4);
            IntegerDefinition<Int64>.Size.ShouldBe(8);
            IntegerDefinition<byte>.Size.ShouldBe(1);
            IntegerDefinition<UInt16>.Size.ShouldBe(2);
            IntegerDefinition<UInt32>.Size.ShouldBe(4);
            IntegerDefinition<UInt64>.Size.ShouldBe(8);
        }
        
    }
}