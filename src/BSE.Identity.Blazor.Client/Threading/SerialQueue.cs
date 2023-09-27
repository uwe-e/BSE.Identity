/*
 * SerialQueue
 * 
 * Lightweight C# Task-based implementation of FIFO serial queues from ObjC, which are often much better to use for
 * synchronization rather than locks - they don't block caller's thread, and rather than creating new threads - they use thread pool.
 * 
 * Original Code https://github.com/gentlee/SerialQueue
 * 
 */

namespace BSE.Identity.Blazor.Client.Threading
{
    public class SerialQueue
    {
        readonly object _locker = new object();
        readonly WeakReference<Task> _lastTask = new WeakReference<Task>(null);

        public Task Enqueue(Action action)
        {
            return Enqueue<bool>(() =>
            {
                action();
                return true;
            });
        }

        public Task<T> Enqueue<T>(Func<T> function)
        {
            lock (_locker)
            {
                Task lastTask;
                Task<T> resultTask;

                if (_lastTask.TryGetTarget(out lastTask))
                {
                    resultTask = lastTask.ContinueWith(_ => function(), TaskContinuationOptions.ExecuteSynchronously);
                }
                else
                {
                    resultTask = Task.Run(function);
                }

                _lastTask.SetTarget(resultTask);

                return resultTask;
            }
        }

        public Task Enqueue(Func<Task> asyncAction)
        {
            lock (_locker)
            {
                Task lastTask;
                Task resultTask;

                if (_lastTask.TryGetTarget(out lastTask))
                {
                    resultTask = lastTask.ContinueWith(_ => asyncAction(), TaskContinuationOptions.ExecuteSynchronously).Unwrap();
                }
                else
                {
                    resultTask = Task.Run(asyncAction);
                }

                _lastTask.SetTarget(resultTask);

                return resultTask;
            }
        }

        public Task<T> Enqueue<T>(Func<Task<T>> asyncFunction)
        {
            lock (_locker)
            {
                Task lastTask;
                Task<T> resultTask;

                if (_lastTask.TryGetTarget(out lastTask))
                {
                    resultTask = lastTask.ContinueWith(_ => asyncFunction(), TaskContinuationOptions.ExecuteSynchronously).Unwrap();
                }
                else
                {
                    resultTask = Task.Run(asyncFunction);
                }

                _lastTask.SetTarget(resultTask);

                return resultTask;
            }
        }
    }
}
