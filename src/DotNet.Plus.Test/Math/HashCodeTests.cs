using Microsoft.VisualStudio.TestTools.UnitTesting;
using DotNet.Plus.Math;
using System;
using System.Collections.Generic;
using System.Text;
using Shouldly;

namespace DotNet.Plus.Math.Tests
{
    [TestClass()]
    public class HashCodeTests
    {
        [TestMethod()]
        public void HashTest()
        {
            HashCode.Start.Hash(10).ShouldBe(537);
            HashCode.Start.Hash(10, 20, 30).ShouldBe(516707);

            var list = new byte[] {10, 20, 30};
            HashCode.Start.Hash(list).ShouldBe(516707);

        }
    }
}