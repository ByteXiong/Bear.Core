using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BearPlatform.Common.Helper;

/// <summary>
/// 异步转同步,防止ASP.NET中死锁
/// https://stackoverflow.com/questions/5095183/how-would-i-run-an-async-taskt-method-synchronously
/// </summary>
public static class TaskHelper
{
    public static void RunSync(Func<Task> task)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(async _ =>
        {
            try
            {
                await task();
            }
            catch (System.Exception e)
            {
                synch.InnerException = e;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, null);
        synch.BeginMessageLoop();

        SynchronizationContext.SetSynchronizationContext(oldContext);
    }

    public static T RunSync<T>(Func<Task<T>> task)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        T ret = default(T);
        synch.Post(async _ =>
        {
            try
            {
                ret = await task();
            }
            catch (System.Exception e)
            {
                synch.InnerException = e;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, null);
        synch.BeginMessageLoop();
        SynchronizationContext.SetSynchronizationContext(oldContext);
        return ret;
    }

    private class ExclusiveSynchronizationContext : SynchronizationContext
    {
        private bool _done;
        public System.Exception InnerException { get; set; }
        readonly AutoResetEvent _workItemsWaiting = new AutoResetEvent(false);

        readonly Queue<Tuple<SendOrPostCallback, object>> _items =
            new Queue<Tuple<SendOrPostCallback, object>>();

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new NotSupportedException("We cannot send to our same thread");
        }

        public override void Post(SendOrPostCallback d, object state)
        {
            lock (_items)
            {
                _items.Enqueue(Tuple.Create(d, state));
            }

            _workItemsWaiting.Set();
        }

        public void EndMessageLoop()
        {
            Post(_ => _done = true, null);
        }

        public void BeginMessageLoop()
        {
            while (!_done)
            {
                Tuple<SendOrPostCallback, object> task = null;
                lock (_items)
                {
                    if (_items.Count > 0)
                    {
                        task = _items.Dequeue();
                    }
                }

                if (task != null)
                {
                    task.Item1(task.Item2);
                    if (InnerException != null) // the method threw an exeption
                    {
                        throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                    }
                }
                else
                {
                    _workItemsWaiting.WaitOne();
                }
            }
        }

        public override SynchronizationContext CreateCopy()
        {
            return this;
        }
    }
}
