using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ImageProcessing.Utility.DataStructure.BlockingQueueSrc.Implementation
{
    public sealed class BlockingQueue<T> : IDisposable
    {
        private readonly int _maxSize;
        private readonly Queue<T> _queue
            = new Queue<T>();

        private bool _closing;

        public BlockingQueue(int maxSize)
        {
            if(maxSize <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxSize));
            }

            _maxSize = maxSize;
        }

        public bool TryEnqueue(T item)
        {
            lock (_queue)
            {
                if (_closing)
                {
                    return false;
                }

                //if the number of tasks exceeds the max size
                //block the current thread
                while (_queue.Count >= _maxSize)
                {
                    Monitor.Wait(_queue);
                }
                _queue.Enqueue(item);

                //if there is the only 1 task on the queue
                //wake up all blocked threads
                if (_queue.Count == 1)
                {
                    Monitor.PulseAll(_queue);
                }

                return true;
            }
        }

        public bool TryDequeue(out T value)
        {
            lock (_queue)
            {
                //if there is no active tasks
                //and the closing was not called
                //block
                while (_queue.Count == 0)
                {
                    if (_closing)
                    {
                        value = default(T);
                        return false;
                    }
                    Monitor.Wait(_queue);
                }
                value = _queue.Dequeue();

                //if the upper bound is reached
                //wake up all blocked on dequeue threads
                if (_queue.Count == _maxSize - 1)
                {
                    Monitor.PulseAll(_queue);
                }
                return true;
            }
        }

        public bool Any()
        {
            lock(_queue)
            {
                return _queue.Any();
            }
        }

        public void Dispose()
        {
            lock (_queue)
            {
                if (!_closing)
                {
                    _closing = true;
                    Monitor.PulseAll(_queue);
                }
            }

        }
    }
}
