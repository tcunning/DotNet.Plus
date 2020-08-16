using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using DotNet.Plus.BasicType;
using Shouldly;

namespace DotNet.Plus.Test.BasicType
{
    [TestClass]
    public class ByteArrayTests
    {
        [TestMethod]
        public void ArraySegmentTest()
        {
            var bufferSource = new byte[] { 0x01, 0x02, 0x03, 0x04 };
            ((IList<byte>)bufferSource.ToArraySegment(0)).ShouldBe(new List<byte>() { 0x01, 0x02, 0x03, 0x04 });
            ((IList<byte>)bufferSource.ToArraySegment(1)).ShouldBe(new List<byte>() { 0x02, 0x03, 0x04 });
            ((IList<byte>)bufferSource.ToArraySegment(2)).ShouldBe(new List<byte>() { 0x03, 0x04 });
            ((IList<byte>)bufferSource.ToArraySegment(3)).ShouldBe(new List<byte>() { 0x04 });

            ((IList<byte>)bufferSource.ToArraySegment(1, 2)).ShouldBe(new List<byte>() { 0x02, 0x03 });
            ((IList<byte>)bufferSource.ToArraySegment(2, 2)).ShouldBe(new List<byte>() { 0x03, 0x04 });
        }

        [TestMethod]
        public void FillTest()
        {
            new byte[4].Fill(0xFF).ShouldBe(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF });
            new byte[4].Fill(0x22).ShouldBe(new byte[] { 0x22, 0x22, 0x22, 0x22 });

            new byte[] { 0x01, 0x02, 0x03, 0x04 }.Fill(0xFF, 1, 2).ShouldBe(new byte[] { 0x01, 0xFF, 0xFF, 0x04 });
            new byte[] { 0x01, 0x02, 0x03, 0x04 }.Fill(0xFF, 0, 0).ShouldBe(new byte[] { 0x01, 0x02, 0x03, 0x04 });

            Should.Throw<ArgumentOutOfRangeException>(() => new byte[4].Fill(0xFF, -1));
            Should.Throw<ArgumentOutOfRangeException>(() => new byte[4].Fill(0xFF, -1, 4));
            Should.Throw<ArgumentOutOfRangeException>(() => new byte[4].Fill(0xFF, 0, -1));
        }

        [TestMethod]
        public void ClearTest()
        {
            (new byte[4]).Clear().ShouldBe(new byte[] { 0x00, 0x00, 0x00, 0x00 });
        }

    }
}