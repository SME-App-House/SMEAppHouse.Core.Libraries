﻿using System;
using System.Threading;

namespace SMEAppHouse.Core.ProcessService.Specials
{
    /// <summary>Event arguments representing the completion of a parallel action.</summary>
    public class ParallelEventArgs : EventArgs
    {
        private readonly object _state;
        private readonly Exception _exception;
        internal ParallelEventArgs(object state, Exception exception)
        {
            _state = state;
            _exception = exception;
        }

        /// <summary>The opaque state object that identifies the action (null otherwise).</summary>
        public object State { get { return _state; } }

        /// <summary>The exception thrown by the parallel action, or null if it completed without exception.</summary>
        public Exception Exception { get { return _exception; } }
    }

    /// <summary>
    /// Provides a caller-friendly wrapper around parallel actions.
    /// http://stackoverflow.com/questions/540078/wait-for-pooled-threads-to-complete/540380#540380
    /// </summary>
    public sealed class Forker
    {
        int _running;
        private readonly object _joinLock = new object(), _eventLock = new object();

        /// <summary>Raised when all operations have completed.</summary>
        public event EventHandler AllComplete
        {
            add { lock (_eventLock) { _allComplete += value; } }
            remove
            {
                lock (_eventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    if (_allComplete != null) _allComplete -= value;
                }
            }
        }
        private EventHandler _allComplete;
        /// <summary>Raised when each operation completes.</summary>
        public event EventHandler<ParallelEventArgs> ItemComplete
        {
            add { lock (_eventLock) { _itemComplete += value; } }
            remove
            {
                lock (_eventLock)
                {
                    // ReSharper disable once DelegateSubtraction
                    if (_itemComplete != null) _itemComplete -= value;
                }
            }
        }
        private EventHandler<ParallelEventArgs> _itemComplete;

        private void OnItemComplete(object state, Exception exception)
        {
            EventHandler<ParallelEventArgs> itemHandler = _itemComplete; // don't need to lock
            if (itemHandler != null) itemHandler(this, new ParallelEventArgs(state, exception));
            if (Interlocked.Decrement(ref _running) == 0)
            {
                EventHandler allHandler = _allComplete; // don't need to lock
                if (allHandler != null) allHandler(this, EventArgs.Empty);
                lock (_joinLock)
                {
                    Monitor.PulseAll(_joinLock);
                }
            }
        }

        /// <summary>Adds a callback to invoke when each operation completes.</summary>
        /// <returns>Current instance (for fluent API).</returns>
        public Forker OnItemComplete(EventHandler<ParallelEventArgs> handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            ItemComplete += handler;
            return this;
        }

        /// <summary>Adds a callback to invoke when all operations are complete.</summary>
        /// <returns>Current instance (for fluent API).</returns>
        public Forker OnAllComplete(EventHandler handler)
        {
            if (handler == null) throw new ArgumentNullException("handler");
            AllComplete += handler;
            return this;
        }

        /// <summary>Waits for all operations to complete.</summary>
        public void Join()
        {
            Join(-1);
        }

        /// <summary>Waits (with timeout) for all operations to complete.</summary>
        /// <returns>Whether all operations had completed before the timeout.</returns>
        public bool Join(int millisecondsTimeout)
        {
            lock (_joinLock)
            {
                if (CountRunning() == 0) return true;
                Thread.SpinWait(1); // try our luck...
                return (CountRunning() == 0) ||
                    Monitor.Wait(_joinLock, millisecondsTimeout);
            }
        }

        /// <summary>Indicates the number of incomplete operations.</summary>
        /// <returns>The number of incomplete operations.</returns>
        public int CountRunning()
        {
            return Interlocked.CompareExchange(ref _running, 0, 0);
        }

        /// <summary>Enqueues an operation.</summary>
        /// <param name="action">The operation to perform.</param>
        /// <returns>The current instance (for fluent API).</returns>
        public Forker Fork(ThreadStart action) { return Fork(action, null); }

        /// <summary>Enqueues an operation.</summary>
        /// <param name="action">The operation to perform.</param>
        /// <param name="state">An opaque object, allowing the caller to identify operations.</param>
        /// <returns>The current instance (for fluent API).</returns>
        public Forker Fork(ThreadStart action, object state)
        {
            if (action == null) throw new ArgumentNullException("action");
            Interlocked.Increment(ref _running);
            ThreadPool.QueueUserWorkItem(delegate
            {
                Exception exception = null;
                try { action(); }
                catch (Exception ex) { exception = ex; }
                OnItemComplete(state, exception);
            });
            return this;
        }
    }
}
