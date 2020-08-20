using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace DotNet.Plus.Collection
{
    /// <summary>
    /// A concurrent FIFO queue that supports a maximum queue size.
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    public class QueueFifoConcurrent<TItem> : IProducerConsumerCollection<TItem>, IReadOnlyCollection<TItem>
    {
        private readonly ConcurrentQueue<TItem> _queue;

        /// <summary>
        /// Maximum size of the queue
        /// </summary>
        public int LimitSize { get; }

        /// <summary>
        /// Options associated with the queue <see cref="QueueOption"/>
        /// </summary>
        public QueueOption Option { get; }

        /// <summary>
        /// Creates a new FIFO queue
        /// </summary>
        /// <param name="limit">The maximum number of items allocated in the queue</param>
        /// <param name="queueOption">The options associated with the queue <see cref="QueueOption"/></param>
        public QueueFifoConcurrent(int limit, QueueOption queueOption = QueueOption.None) : this(limit, null, queueOption)
        {
        }

        /// <summary>
        /// Creates a new FIFO queue
        /// </summary>
        /// <param name="limit">The maximum number of items allocated in the queue</param>
        /// <param name="collection">Queue is filled with items from the given collection.  All the items from the given collection will
        /// be initially added to the queue, even if there are more then the specified limit.  However, the next time an item is enqueued,
        /// the excess items will be removed until the queue is back within it's size limit.</param>
        /// <param name="queueOption">The options associated with the queue <see cref="QueueOption"/></param>
        public QueueFifoConcurrent(int limit, System.Collections.Generic.IEnumerable<TItem>? collection, QueueOption queueOption = QueueOption.None)
        {
            if( limit <= 0 )
                throw new ArgumentOutOfRangeException(nameof(limit), limit, $"Must be >= 0");

            _queue = collection == null ? new ConcurrentQueue<TItem>() : new ConcurrentQueue<TItem>(collection);
            LimitSize = limit;
            Option = queueOption;
        }

        /// <inheritdoc cref="ConcurrentQueue{TValue}.Count"/>
        public int Count => _queue.Count;

        /// <inheritdoc cref="ICollection.IsSynchronized"/>
        bool ICollection.IsSynchronized => ((ICollection)_queue).IsSynchronized;

        /// <inheritdoc cref="ICollection.SyncRoot"/>
        object ICollection.SyncRoot => ((ICollection)_queue).SyncRoot;

        /// <inheritdoc cref="ConcurrentQueue{TValue}.IsEmpty"/>
        public bool IsEmpty => _queue.IsEmpty;

        /// <summary>
        /// Returns true is the queue is full 
        /// </summary>
        public bool IsFull => _queue.Count >= LimitSize;

        /// <summary>
        /// Clears the items from the queue
        /// </summary>
        public void Clear()
        {
            lock ( ((ICollection)this).SyncRoot )
            {
                //_queue.Clear();           // TODO: Supported in .NET Standard 2.1
                while( _queue.Count > 0 && _queue.TryDequeue(out _) )
                    continue;

                /* we have made the queue as empty as possible */
            }
        }

        /// <inheritdoc cref="ConcurrentQueue{TValue}.CopyTo(TValue[], int)"/>
        public void CopyTo(TItem[] array, int index)
        {
            lock( ((ICollection)this).SyncRoot )
            {
                _queue.CopyTo(array, index);
            }
        }

        /// <inheritdoc cref="ICollection.CopyTo(Array, int)"/>
        void ICollection.CopyTo(Array array, int index)
        {
            lock( ((ICollection)this).SyncRoot )
            {
                ((ICollection)_queue).CopyTo(array, index);
            }
        }

        /// <summary>
        /// <para>Adds the given item to the end of the FIFO queue.  If the queue is full, one of two things will happen based
        /// on the queue options:</para>
        /// <list type="bullet">
        ///     <item>
        ///         <term>QueueOption.None</term>
        ///         <description>The oldest items on the queue will be removed until there is room in the queue for the new item</description>
        ///     </item>
        ///     <item>
        ///         <term>QueueOption.ThrowOnAdd</term>
        ///         <description>An ArgumentOutOfRangeException will be thrown if the queue is full</description>
        ///     </item>
        /// </list>
        /// </summary>
        /// <param name="item">The item to add to the end of the queue</param>
        public void Enqueue(TItem item)
        {
            lock( ((ICollection)this).SyncRoot )
            {
                if( IsFull && Option.HasFlag(QueueOption.ThrowOnAdd) )
                    throw new ArgumentOutOfRangeException(nameof(item), item, $"The queue is full with {_queue.Count} items.");

                while( IsFull && _queue.TryDequeue(out _) )
                    continue;

                if( IsFull )
                    throw new ArgumentOutOfRangeException(nameof(item), item, $"The queue is full with {_queue.Count} items and unable to make room.");

                _queue.Enqueue(item);
            }
        }

        /// <inheritdoc cref="ConcurrentQueue{TValue}.GetEnumerator()"/>
        public IEnumerator<TItem> GetEnumerator() => _queue.GetEnumerator();

        /// <inheritdoc cref="IEnumerable{TItem}.GetEnumerator()"/>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable<TItem>)this).GetEnumerator();

        /// <inheritdoc cref="ConcurrentQueue{TValue}.ToArray()"/>
        public TItem[] ToArray() => _queue.ToArray();

        /// <summary>
        /// <para>Adds the given item to the end of the FIFO queue.</para>
        /// </summary>
        /// <param name="item">The item to add to the end of the queue</param>
        /// <returns>
        /// <para>If the queue is full, one of two things will happen based/// on the queue options:</para>
        /// <list type="bullet">
        ///     <item>
        ///         <term>QueueOption.None</term>
        ///         <description>The oldest items on the queue will be removed until there is room in the queue for the new item and true will be returned</description>
        ///     </item>
        ///     <item>
        ///         <term>QueueOption.ThrowOnAdd</term>
        ///         <description>The item WON'T get added to the queue and false will be returned</description>
        ///     </item>
        /// </list>
        /// </returns>
        public bool TryAdd(TItem item)
        {
            try
            {
                Enqueue(item);
                return true;
            }
            catch( Exception ex )
            {
                return false;
            }
        }

        bool IProducerConsumerCollection<TItem>.TryTake(out TItem item) => TryDequeue(out item);

        /// <inheritdoc cref="ConcurrentQueue{TValue}.TryDequeue(out TValue)"/>
        public bool TryDequeue(out TItem result)
        {
            lock( ((ICollection)this).SyncRoot ) {
                return _queue.TryDequeue(out result);
            }
        }

        /// <inheritdoc cref="ConcurrentQueue{TValue}.TryPeek(out TValue)"/>
        public bool TryPeek(out TItem result) => _queue.TryPeek(out result);
    }
}

