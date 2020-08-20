using System;

namespace DotNet.Plus.Collection
{
    /// <summary>
    /// Configuration options supported by the FIFO queue
    /// </summary>
    [Flags]
    public enum QueueOption
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
}

