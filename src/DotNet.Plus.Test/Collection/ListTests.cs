using System;
using System.Collections.Generic;
using DotNet.Plus.Collection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Collection
{
    [TestClass]
    public class ListTests
    {
        [TestMethod]
        public void TryTakeFirstTest()
        {
            List.TryTakeFirst<int>(null, out var ok).ShouldBe(false);

            var list = new List<int>() { 10, 20, 30, 40 };
        
            list.TryTakeFirst(out var item1).ShouldBe(true);
            item1.ShouldBe(10);
            
            list.TryTakeFirst(out var item2).ShouldBe(true);
            item2.ShouldBe(20);
            
            list.TryTakeFirst(out var item3).ShouldBe(true);
            item3.ShouldBe(30);
            
            list.TryTakeFirst(out var item4).ShouldBe(true);
            item4.ShouldBe(40);

            list.TryTakeFirst(out var item5).ShouldBe(false);
        }
    }
}