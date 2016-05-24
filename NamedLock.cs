using System;
using System.Collections.Concurrent;

namespace DevSumScheduler
{
    public class NamedLock
    {
        private readonly ConcurrentDictionary<string, object> locks = new ConcurrentDictionary<string, object>();

        public object GetLock(string name)
        {
            return this.locks.GetOrAdd(name, _ => new object());
        }

        public TResult RunWithLock<TResult>(string name, Func<TResult> body)
        {
            lock (this.locks.GetOrAdd(name, _ => new object()))
            {
                return body();
            }
        }

        public void RunWithLock(string name, Action body)
        {
            lock (this.locks.GetOrAdd(name, _ => new object()))
            {
                body();
            }
        }

        public void RemoveLock(string name)
        {
            object o;
            this.locks.TryRemove(name, out o);
        }
    }
}