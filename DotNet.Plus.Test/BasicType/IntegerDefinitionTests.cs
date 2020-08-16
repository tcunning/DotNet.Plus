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
            IntegerDefinition<sbyte>.TypeCode.ShouldBe(TypeCode.SByte);
            IntegerDefinition<Int16>.TypeCode.ShouldBe(TypeCode.Int16);
            IntegerDefinition<Int32>.TypeCode.ShouldBe(TypeCode.Int32);
            IntegerDefinition<Int64>.TypeCode.ShouldBe(TypeCode.Int64);
            IntegerDefinition<byte>.TypeCode.ShouldBe(TypeCode.Byte);
            IntegerDefinition<UInt16>.TypeCode.ShouldBe(TypeCode.UInt16);
            IntegerDefinition<UInt32>.TypeCode.ShouldBe(TypeCode.UInt32);
            IntegerDefinition<UInt64>.TypeCode.ShouldBe(TypeCode.UInt64);
        }
    }
}