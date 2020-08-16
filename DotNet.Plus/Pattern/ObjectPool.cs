using System;
using System.Collections.Concurrent;

namespace DotNet.Plus.Pattern
{
    /// <summary>
    /// Provide a simple implementation of an ObjectPool.  Currently, there is no provisions for pre-allocating
    /// objects to the pool, and there are also no limits on how many items can be put into the pool.
    ///
    /// While the object pool is thread safe, it doesn't prevent duplicate objects in the pool.  So use the
    /// ObjectPool wisely!
    ///
    /// If disposable objects are placed in the ObjectPool, the ObjectPool will NOT attempt to dispose the object.
    /// 
    /// See https://docs.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool
    /// </summary>
    /// <typeparam name="TObject">Type of the objects in the pool.  The object's must be classes as it doesn't make
    /// sense to put structs in the pool because they would just be copied.</typeparam>
    public class ObjectPool<TObject>
        where TObject : class
    {
        private readonly ConcurrentBag<TObject> _objectPool;
        private readonly Func<TObject> _objectGenerator;

        /// <summary>
        /// Get's the number of objects currently in the pool.
        /// </summary>
        public int CurrentInPool => _objectPool.Count;

        /// <summary>
        /// Total created objects for the lifetime of the object pool
        /// </summary>
        public int TotalCreated { get; private set; } = 0;

        /// <summary>
        /// Create an object pool with objects that will be create via the default constructor.
        /// </summary>
        /// <typeparam name="TNewObject">Type of the object that supports a default constructor</typeparam>
        /// <returns></returns>
        public static ObjectPool<TObject> MakeObjectPool<TNewObject>() 
            where TNewObject : TObject, new() => new ObjectPool<TObject>(() => new TNewObject());

        /// <summary>
        /// Create an object pool with objects that will be created with the passed in objectGenerator.
        /// </summary>
        /// <param name="objectGenerator">Should return a new/initialized object that will be placed in the pool
        /// or handed out in response to a TakeObject</param>
        /// <returns></returns>
        public static ObjectPool<TObject> MakeObjectPool(Func<TObject> objectGenerator) => new ObjectPool<TObject>(objectGenerator);

        /// <summary>
        /// Creates an object pool with the given object Generator
        /// </summary>
        /// <param name="objectGenerator">Should return a new/initialized object that will be placed in the pool
        /// or handed out in response to a TakeObject</param>
        public ObjectPool(Func<TObject> objectGenerator)
        {
            _objectPool = new ConcurrentBag<TObject>();
            _objectGenerator = objectGenerator ?? throw new ArgumentNullException(nameof(objectGenerator));
        }
        
        /// <summary>
        /// Takes an object from the pool.  If there are none in the pool, it will create a new object.
        /// </summary>
        /// <returns>An newly created object or one reused from the object pool</returns>
        public TObject TakeObject()
        {
            if( _objectPool.TryTake(out var item) )
                return item;

            TotalCreated += 1;
            return _objectGenerator();
        }

        /// <summary>
        /// Put an object into the pool for reuse.  It is the responsibility of caller to make sure the object is put back
        /// in a state that allows it to be reused.  The taker of an object, is not required to ever return the object back
        /// to the pool.
        ///
        /// The ObjectPool allows duplicate objects to be put into the pool, so it's up to the caller to guarantee that
        /// object's added to the pool are unique instances (if that is the desired result).
        /// </summary>
        /// <param name="item"></param>
        /// <exception cref="ObjectDisposedException">Will be thrown if we can detect the item was disposed</exception>
        public void PutObject(TObject item)
        {
            if( item == null )
                return;

            if( item is ICommonDisposable disposableItem && disposableItem.IsDisposed )
                throw new ObjectDisposedException(item.GetType().Name);

            _objectPool.Add(item);
        }

        public override string ToString()
        {
            return $"ObjectPool<{typeof(TObject).Name}>: Total Created {TotalCreated} Total Unused {CurrentInPool}";
        }
    }
}
