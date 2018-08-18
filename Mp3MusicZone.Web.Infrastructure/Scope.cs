namespace Mp3MusicZone.Web.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Scope : IDisposable
    {
        private IDictionary<Type, object> cache;
        private IList<IDisposable> disposables;

        public Scope()
        {
            this.cache = new Dictionary<Type, object>();
            this.disposables = new List<IDisposable>();
        }

        public T Get<T>(Func<Scope, T> factory)
        {
            if (!this.cache.ContainsKey(typeof(T)))
            {
                T instance = factory.Invoke(this);
                this.cache[typeof(T)] = instance;

                if (instance is IDisposable)
                {
                    this.disposables.Add((IDisposable)instance);
                }
            }

            return (T)this.cache[typeof(T)];
        }

        public void Dispose()
        {
            IEnumerable<IDisposable> reversed = this.disposables.Reverse();

            foreach (var disposable in reversed)
            {
                disposable.Dispose();
            }
        }
    }
}
