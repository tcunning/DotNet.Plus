using System;
using DotNet.Plus.Pattern;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotNet.Plus.Test.Pattern
{
    [TestClass]
    public class ObjectPoolTests
    {
        private static int _testObjectCount = 0;
        class TestObject : CommonDisposable
        {
            public readonly int Id;
            public TestObject() => Id = ++_testObjectCount;
            public override void Dispose(bool disposing) { }
        }

        class TestObjectCustom : CommonDisposable
        {
            public readonly int Id;
            public TestObjectCustom(int id) => Id = id;
            public override void Dispose(bool disposing) { }
        }

        [TestMethod]
        public void MakeObjectPoolTest()
        {
            var objectPool = ObjectPool<TestObject>.MakeObjectPool<TestObject>();

            var obj1 = objectPool.TakeObject();
            obj1.ShouldNotBeNull();
            obj1.Id.ShouldBe(1);
            objectPool.CurrentInPool.ShouldBe(0);
            objectPool.TotalCreated.ShouldBe(1);

            var obj2 = objectPool.TakeObject();
            obj2.ShouldNotBeNull();
            obj2.Id.ShouldBe(2);
            objectPool.CurrentInPool.ShouldBe(0);
            objectPool.TotalCreated.ShouldBe(2);

            objectPool.PutObject(obj1);
            objectPool.CurrentInPool.ShouldBe(1);

            objectPool.PutObject(obj2);
            objectPool.CurrentInPool.ShouldBe(2);

            var objReusedA = objectPool.TakeObject();
            objReusedA.ShouldBeOneOf(obj1, obj2);
            objectPool.CurrentInPool.ShouldBe(1);

            var objReusedB = objectPool.TakeObject();
            objReusedB.ShouldBeOneOf(obj1, obj2);
            objectPool.CurrentInPool.ShouldBe(0);

            var obj3 = objectPool.TakeObject();
            obj3.ShouldNotBeOneOf(obj1, obj2);
            obj3.TryDispose();
            Should.Throw<ObjectDisposedException>(() => objectPool.PutObject(obj3));

            objectPool.PutObject(null);
            objectPool.ToString().Length.ShouldBeGreaterThan(0);

            objectPool.TotalCreated.ShouldBe(3);
        }
        
        [TestMethod]
        public void MakeObjectPoolCustomTest()
        {
            int objectId = 0;

            Should.Throw<ArgumentNullException>(() => new ObjectPool<TestObjectCustom>(null));
            
            var objectPool = ObjectPool<TestObjectCustom>.MakeObjectPool(() => new TestObjectCustom(++objectId));

            var obj1 = objectPool.TakeObject();
            obj1.ShouldNotBeNull();
            obj1.Id.ShouldBe(1);
            objectPool.CurrentInPool.ShouldBe(0);
            objectPool.TotalCreated.ShouldBe(1);

            var obj2 = objectPool.TakeObject();
            obj2.ShouldNotBeNull();
            obj2.Id.ShouldBe(2);
            objectPool.CurrentInPool.ShouldBe(0);
            objectPool.TotalCreated.ShouldBe(2);

            objectPool.PutObject(obj1);
            objectPool.CurrentInPool.ShouldBe(1);

            objectPool.PutObject(obj2);
            objectPool.CurrentInPool.ShouldBe(2);

            var objReusedA = objectPool.TakeObject();
            objReusedA.ShouldBeOneOf(obj1, obj2);
            objectPool.CurrentInPool.ShouldBe(1);

            var objReusedB = objectPool.TakeObject();
            objReusedB.ShouldBeOneOf(obj1, obj2);
            objectPool.CurrentInPool.ShouldBe(0);

            var obj3 = objectPool.TakeObject();
            obj3.ShouldNotBeOneOf(obj1, obj2);
            obj3.TryDispose();
            Should.Throw<ObjectDisposedException>(() => objectPool.PutObject(obj3));

            objectPool.PutObject(null);
            objectPool.ToString().Length.ShouldBeGreaterThan(0);

            objectPool.TotalCreated.ShouldBe(3);
        }

    }
}