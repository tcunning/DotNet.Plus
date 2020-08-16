using System;
using DotNet.Plus.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Core
{
    [TestClass]
    public class ConvertUncheckedTests
    {
        [TestMethod]
        public void ChangeTypeTest()
        {
            ConvertUnchecked.ChangeType(null, TypeCode.Boolean).ShouldBeNull();

            Should.Throw<InvalidCastException>(() => ConvertUnchecked.ChangeType(new object(), TypeCode.Boolean));

            Should.Throw<ArgumentOutOfRangeException>(() => ConvertUnchecked.ChangeType(12345, (TypeCode)0xFF));

            ConvertUnchecked.ChangeType<Char>(0x41).ShouldBe('A');

            ConvertUnchecked.ChangeType(0x41, typeof(Char)).ShouldBe('A');

            Should.Throw<ArgumentException>(() => ConvertUnchecked.ChangeType<bool>(DateTime.Now));
        }
    }
}