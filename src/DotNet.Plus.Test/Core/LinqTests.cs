using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Shouldly;

namespace DotNet.Plus.Core.Tests
{
    [TestClass()]
    public class LinqTests
    {
        [TestMethod()]
        public void WhereNotTest()
        {
            var list = new[] {2, 3, 4, 6, 7, 8};
            list.WhereNot((item) => item > 6).ShouldBe(new[] { 2, 3, 4, 6 });
        }
    }
}