using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using Shouldly;
using DotNet.Plus.Core;

namespace DotNet.Plus.Collection.Tests
{
    [TestClass]
    public class QueueFifoConcurrentTests
    {
        [TestMethod]
        public void EnqueueTest()
        {
            var queue = new QueueFifoConcurrent<int>(limit: 3, QueueOption.None);
            queue.Option.ShouldBe(QueueOption.None);

            queue.TryPeek(out var peek0).ShouldBe(false);
            peek0.ShouldBe(0);

            queue.IsEmpty.ShouldBe(true);
            queue.Count.ShouldBe(0);
            queue.Enqueue(1);
            queue.IsEmpty.ShouldBe(false);

            queue.Count.ShouldBe(1);
            queue.TryAdd(2);
            queue.Count.ShouldBe(2);
            queue.Enqueue(3);
            queue.Count.ShouldBe(3);
            queue.Enqueue(4);
            queue.Count.ShouldBe(3);
            queue.IsFull.ShouldBe(true);

            queue.TryPeek(out var peek2).ShouldBe(true);
            peek2.ShouldBe(2);

            queue.ToArray().ShouldBe(new [] {2, 3, 4});

            queue.Select((t) => t).ToList().ShouldBe(new[] { 2, 3, 4 });

            ((IEnumerable) queue).ToEnumerable<int>().ToList().ShouldBe(new[] {2, 3, 4});

            ((IProducerConsumerCollection<int>)queue).TryTake(out var item2).ShouldBe(true);
            item2.ShouldBe(2);
            queue.TryDequeue(out var item3).ShouldBe(true);
            item3.ShouldBe(3);
            queue.TryDequeue(out var item4).ShouldBe(true);
            item4.ShouldBe(4);
            queue.TryDequeue(out var item5).ShouldBe(false);
            queue.Count.ShouldBe(0);
        }

        [TestMethod]
        public void EnqueueClearTest()
        {
            var queue = new QueueFifoConcurrent<int>(limit: 3, QueueOption.None);
            queue.Enqueue(1);
            queue.Enqueue(1);
            queue.Enqueue(1);
            queue.Count.ShouldBe(3);
            queue.Clear();
            queue.Count.ShouldBe(0);
        }

        [TestMethod]
        public void EnqueueCopyToTest()
        {
            var queue = new QueueFifoConcurrent<int>(limit: 3, QueueOption.None);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var copy = new int[3];
            queue.CopyTo(copy, 0);
            copy.ShouldBe(new[] { 1, 2, 3 });

            var copy2 = new int[4];
            queue.CopyTo(copy2, 1);
            copy2.ShouldBe(new[] {0, 1, 2, 3 });

            var copy3 = new int[4];
            ((ICollection)queue).CopyTo(copy2, 1);
            copy2.ShouldBe(new[] { 0, 1, 2, 3 });
        }
        
        [TestMethod]
        public void EnqueueFailuresTest()
        {
            var queue = new QueueFifoConcurrent<int>(limit: 3, QueueOption.None);
            ((ICollection)queue).IsSynchronized.ShouldBe(false);
            Should.Throw<NotSupportedException>(() => _ = ((ICollection)queue).SyncRoot);

            Should.Throw<ArgumentOutOfRangeException>(() => _ = new QueueFifoConcurrent<int>(limit: 0, QueueOption.None));
        }

        [TestMethod]
        public void EnqueueFullTest()
        {
            var queue = new QueueFifoConcurrent<int>(limit: 3, QueueOption.None);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            queue.IsFull.ShouldBe(true);

            queue.SetProperty<bool>("EnableAutoDequeue", false, BindingFlags.NonPublic);
            queue.GetProperty<bool>("EnableAutoDequeue", BindingFlags.NonPublic).ShouldBe(false);
            Should.Throw<ArgumentOutOfRangeException>(() => queue.Enqueue(4));
            queue.SetProperty<bool>("EnableAutoDequeue", true, BindingFlags.NonPublic);
        }


        [TestMethod]
        public void EnqueueFullThrowTest()
        {
            var queue = new QueueFifoConcurrent<int>(limit: 3, QueueOption.ThrowOnFull);
            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);
            Should.Throw<ArgumentOutOfRangeException>(() => queue.Enqueue(4));
        }

    }

}