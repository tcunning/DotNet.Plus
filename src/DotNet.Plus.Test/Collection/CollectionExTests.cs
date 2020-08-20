using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Shouldly;

namespace DotNet.Plus.Collection.Tests
{
    [TestClass]
    public class CollectionExTests
    {
        [TestMethod]
        public void TryGetValueAtIndexTest()
        {
            CollectionEx.TryGetValueAtIndex<int>(null, 0, out var ok).ShouldBe(false);

            var list1 = new int[] {10, 20, 30};
            list1.TryGetValueAtIndex(0, out var item0).ShouldBe(true);
            item0.ShouldBe(10);

            list1.TryGetValueAtIndex(1, out var item1).ShouldBe(true);
            item1.ShouldBe(20);

            list1.TryGetValueAtIndex(2, out var item2).ShouldBe(true);
            item2.ShouldBe(30);

            list1.TryGetValueAtIndex(3, out var item3).ShouldBe(false);
            item3.ShouldBe(0);
        }

        [TestMethod]
        public void TryGetValueAtIndexObjTest()
        {
            CollectionEx.TryGetValueAtIndex<int>(null, (object)0, out var ok).ShouldBe(false);

            var list1 = new int[] { 10, 20, 30 };
            list1.TryGetValueAtIndex(0f, out var item0).ShouldBe(true);
            item0.ShouldBe(10);

            list1.TryGetValueAtIndex(1.0d, out var item1).ShouldBe(true);
            item1.ShouldBe(20);

            list1.TryGetValueAtIndex((object)2, out var item2).ShouldBe(true);
            item2.ShouldBe(30);

            list1.TryGetValueAtIndex((object)3, out var item3).ShouldBe(false);
            item3.ShouldBe(0);

            list1.TryGetValueAtIndex(new object(), out var item4).ShouldBe(false);
            item3.ShouldBe(0);
        }

        [TestMethod]
        public void TryRemoveTest()
        {
            CollectionEx.TryRemove<int>(null, 10).ShouldBe(false);

            var list1 = new List<int>() { 10, 20, 30 };
            list1.TryRemove(20).ShouldBe(true);
            list1.TryRemove(20).ShouldBe(true);
            list1.TryRemove(10).ShouldBe(true);
            list1.TryRemove(30).ShouldBe(true);
        }

    }
}