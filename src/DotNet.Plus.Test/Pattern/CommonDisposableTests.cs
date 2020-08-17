using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Cryptography.X509Certificates;
using DotNet.Plus.Core;
using DotNet.Plus.Pattern;
using Shouldly;

namespace DotNet.Plus.Test.Pattern
{
    [TestClass]
    public class CommonDisposableTests
    {
        class DisposableTest : IDisposable
        {
            private bool _isDisposed = false;

            public void Dispose()
            {
                if( _isDisposed )
                    throw new ObjectDisposedException(nameof(DisposableTest));

                _isDisposed = true;
            }
        }
        
        public class NeverSetBool : ISetOnceValue<bool>
        {
            public bool IsSet => false;

            public bool Value => default;

            public bool SetOnce(bool value) => false;
        }
        
        public class CommonDisposableTest2 : CommonDisposable
        {
            public CommonDisposableTest2()
            {
            }

            public CommonDisposableTest2(ISetOnceValue<bool> setOnce) : base(setOnce)
            {
            }

            public override void Dispose(bool disposing)
            {
            }
        }

        [TestMethod]
        public void TryDisposeTest()
        {
            var t1 = new DisposableTest();
            t1.TryDispose();
            t1.TryDispose();
            Should.Throw<ObjectDisposedException>(() => t1.Dispose());

            var t2 = new CommonDisposableTest2();
            t2.TryDispose();
            t2.TryDispose();
            Should.Throw<ObjectDisposedException>(() => t2.Dispose());

            // In order to get 100% test coverage of all paths we force an exception to get generated 
            // from Dispose and eaten by TryDispose because we never allow the set disposed via doing DI
            // of ISetOnceValue.
            // 
            var t2Test = new CommonDisposableTest2(new NeverSetBool());
            Should.Throw<ObjectDisposedException>(() => t2Test.Dispose());  // Should report an exception
            t2Test.TryDispose();                                                  // Test that an exception didn't get thrown!
        }

        private class CommonDisposableTest : CommonDisposable
        {
            public CommonDisposableTest(ISetOnceValue<bool> setOnceValue) :
                base(setOnceValue)
            {
            }

            public override void Dispose(bool disposing)
            {
            }
        }

        [TestMethod]
        public void InitTest()
        {
            var setOnce = new SetOnceValue<bool>();
            setOnce.SetOnce(true).ShouldBe(true);
            Should.Throw<ArgumentException>(() => new CommonDisposableTest(setOnce));
        }
    }
}