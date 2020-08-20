using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Plus.Collection;
using System.Collections.Generic;
using Shouldly;

namespace DotNet.Plus.Test.Collection
{
    [TestClass]
    public class HashSetExTests
    {
        [TestMethod]
        public void HashSetEqualsTest()
        {
            HashSetEx.HashSetEquals<int>(null, null).ShouldBe(true);

            var set1 = new HashSet<int>() {1, 10, 20};
            var set2 = new HashSet<int>() {10, 1, 20};
            set1.HashSetEquals(set2).ShouldBe(true);
            set1.HashSetEquals(null).ShouldBe(false);

            var set3 = new HashSet<int>() { 10, 1, 20, 30 };
            set1.HashSetEquals(set3).ShouldBe(false);
        }
    }
}