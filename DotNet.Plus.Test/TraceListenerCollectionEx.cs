using System;
using System.Diagnostics;

namespace DotNet.Essentials.BasicType.Tests
{
    public static class TraceListenerCollectionEx
    {
        /// <summary>
        /// This is a helper class that allows us to suspend asserts / all trace listeners
        /// </summary>
        public class SuspendTrackerDisposable : IDisposable
        {
            private readonly TraceListenerCollection _traceListenerCollection;
            private readonly TraceListener[] _suspendedListeners;

            public SuspendTrackerDisposable(TraceListenerCollection traceListenerCollection)
            {
                _traceListenerCollection = traceListenerCollection;

                var numListeners = traceListenerCollection.Count;
                _suspendedListeners = new TraceListener[numListeners];
                for( int index = 0; index < numListeners; index += 1 )
                    _suspendedListeners[index] = traceListenerCollection[index];

                traceListenerCollection.Clear();
            }

            public void Dispose()
            {
                _traceListenerCollection.AddRange(_suspendedListeners);
            }
        }

        public static SuspendTrackerDisposable AssertSuspend(this TraceListenerCollection traceListenerCollection) =>
            new SuspendTrackerDisposable(traceListenerCollection);
    }

}