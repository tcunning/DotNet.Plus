using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DotNet.Plus.Collection
{
    public class ConcurrentFifoQueue<TValue> : IProducerConsumerCollection<TValue>, IReadOnlyCollection<TValue>
    {
        [Flags]
        public enum Option
        {
            /// <summary>
            /// No option specified, will automatically remove oldest item in the queue when a new item is added
            /// and the queue limit has been reached.
            /// </summary>
            None = 0x0000,

            /// <summary>
            /// If this option is provided, an ArgumentOutOfRangeException will be thrown if an Enqueue is attempted on
            /// a full queue.
            /// </summary>
            ThrowOnAdd = 0x0001,
        }

        private readonly ConcurrentQueue<TValue> _queue;

        private readonly object _syncObject = new object();

        public int LimitSize { get; }

        public Option QueueOption { get; }

        public ConcurrentFifoQueue(int limit, Option option = Option.None) : this(limit, null, option)
        {
        }

        public ConcurrentFifoQueue(int limit, System.Collections.Generic.IEnumerable<TValue>? collection, Option option = Option.None)
        {
            if( limit <= 0 )
                throw new ArgumentOutOfRangeException(nameof(limit), limit, $"Must be >= 0");

            _queue = collection == null ? new ConcurrentQueue<TValue>() : new ConcurrentQueue<TValue>(collection);
            LimitSize = limit;
            QueueOption = option;
        }

        public int Count => _queue.Count;

        bool ICollection.IsSynchronized => ((ICollection)_queue).IsSynchronized;

        object ICollection.SyncRoot => ((ICollection)_queue).SyncRoot;

        public bool IsEmpty => _queue.IsEmpty;

        public bool IsFull => _queue.Count >= LimitSize;

        public void Clear()
        {
            lock (_syncObject)
            {
                //_queue.Clear();           // TODO: Supported in .NET Standard 2.1
                while( _queue.Count > 0 && _queue.TryDequeue(out _) )
                    continue;

                /* we have made the queue as empty as possible */
            }
        }

        public void CopyTo(TValue[] array, int index)
        {
            lock (_syncObject) {
                _queue.CopyTo(array, index);
            }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            lock (_syncObject) {
                ((ICollection)_queue).CopyTo(array, index);
            }
        }

        public void Enqueue(TValue obj)
        {
            lock( _syncObject )
            {
                if( IsFull && QueueOption.HasFlag(Option.ThrowOnAdd) )
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, $"The queue is full with {_queue.Count} items.");

                while( IsFull && _queue.TryDequeue(out _) )
                    continue;

                if( IsFull )
                    throw new ArgumentOutOfRangeException(nameof(obj), obj, $"The queue is full with {_queue.Count} items and unable to make room.");

                _queue.Enqueue(obj);
            }
        }

        public IEnumerator<TValue> GetEnumerator() => _queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TValue>)this).GetEnumerator();

        public TValue[] ToArray() => _queue.ToArray();

        public bool TryAdd(TValue item)
        {
            Enqueue(item);
            return true;
        }

        bool IProducerConsumerCollection<TValue>.TryTake(out TValue item) => TryDequeue(out item);

        public bool TryDequeue(out TValue result)
        {
            lock (_syncObject) {
                return _queue.TryDequeue(out result);
            }
        }

        public bool TryPeek(out TValue result) => _queue.TryPeek(out result);
    }
}

